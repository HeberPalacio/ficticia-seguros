import { Component, OnInit, HostListener } from '@angular/core';
import { Router } from '@angular/router';
import { Api, Persona } from '../../services/api';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-personas',
  templateUrl: './personas.component.html',
  styleUrls: ['./personas.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
})
export class PersonasComponent implements OnInit {
  personas: Persona[] = [];
  personasFiltradas: Persona[] = [];

  // Filtros
  mostrarFiltros = false;
  terminoBusqueda: string = '';
  filtroGenero: string = '';
  filtroActivo: string = '';
  filtroLentes: boolean = false;
  filtroDiabetico: boolean = false;
  filtroManeja: boolean = false;

  errorMensaje = '';
  personaSeleccionada: Persona | null = null;
  personaForm!: FormGroup;
  formularioEnviado = false;
  esAdmin: boolean = false;
  modalAbierto: boolean = false;

  // Paginación
  paginaActual: number = 1;
  tamanioPagina: number = 6;
  totalPaginas: number = 1;

  // Expansión de filas
  expandedRow: number | null = null;

  constructor(
    public api: Api,
    private router: Router,
    private fb: FormBuilder,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.cargarPersonas();
    this.initForm();
    this.esAdmin = this.authService.estaLogueado();
  }

  @HostListener('document:click', ['$event'])
  clickFuera(event: Event) {
    const target = event.target as HTMLElement;
    const panel = document.querySelector('.panel-filtros');
    const boton = document.querySelector('.btn-filtros');

    if (
      this.mostrarFiltros &&
      panel &&
      boton &&
      !panel.contains(target) &&
      !boton.contains(target)
    ) {
      this.mostrarFiltros = false;
    }
  }

  toggleExpand(id: number) {
    this.expandedRow = this.expandedRow === id ? null : id;
  }

  initForm() {
    this.personaForm = this.fb.group({
      nombreCompleto: [
        '',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(100),
        ],
      ],
      identificacion: ['', [Validators.required, Validators.maxLength(20)]],
      edad: [
        null,
        [Validators.required, Validators.min(0), Validators.max(120)],
      ],
      genero: ['', Validators.required],
      estadoActivo: [true],
      maneja: [false],
      usaLentes: [false],
      diabetico: [false],
      enfermedadOtra: ['', Validators.maxLength(200)],
    });
  }

  cargarPersonas() {
    this.api.getPersonas().subscribe({
      next: (data) => {
        this.personas = data.sort((a, b) => b.id - a.id);
        this.personasFiltradas = [...this.personas];
        this.actualizarPaginacion();
      },
      error: (err) => {
        if (err.status === 401) {
          this.router.navigate(['/login']);
        } else {
          this.errorMensaje = err.message || 'Error al cargar personas';
        }
      },
    });
  }

  filtrarPersonas(resetPagina: boolean = true) {
    this.personasFiltradas = this.personas.filter((p) => {
      const coincideBusqueda =
        !this.terminoBusqueda ||
        p.nombreCompleto
          .toLowerCase()
          .includes(this.terminoBusqueda.toLowerCase()) ||
        p.identificacion
          .toLowerCase()
          .includes(this.terminoBusqueda.toLowerCase()) ||
        (p.enfermedadOtra &&
          p.enfermedadOtra
            .toLowerCase()
            .includes(this.terminoBusqueda.toLowerCase()));

      const coincideGenero =
        !this.filtroGenero || p.genero === this.filtroGenero;

      const coincideActivo =
        this.filtroActivo === ''
          ? true
          : p.estadoActivo === (this.filtroActivo === 'true');

      const coincideLentes = !this.filtroLentes || p.usaLentes;
      const coincideDiabetico = !this.filtroDiabetico || p.diabetico;
      const coincideManeja = !this.filtroManeja || p.maneja;

      return (
        coincideBusqueda &&
        coincideGenero &&
        coincideActivo &&
        coincideLentes &&
        coincideDiabetico &&
        coincideManeja
      );
    });

    if (resetPagina) {
      this.paginaActual = 1;
    }

    this.actualizarPaginacion();
  }

  limpiarFiltros() {
    this.terminoBusqueda = '';
    this.filtroGenero = '';
    this.filtroActivo = '';
    this.filtroLentes = false;
    this.filtroDiabetico = false;
    this.filtroManeja = false;
    this.filtrarPersonas();
  }

  actualizarPaginacion() {
    this.totalPaginas =
      Math.ceil(this.personasFiltradas.length / this.tamanioPagina) || 1;
    if (this.paginaActual > this.totalPaginas) {
      this.paginaActual = this.totalPaginas;
    }
  }

  get personasPaginadas(): Persona[] {
    const inicio = (this.paginaActual - 1) * this.tamanioPagina;
    return this.personasFiltradas.slice(inicio, inicio + this.tamanioPagina);
  }

  paginaAnterior() {
    if (this.paginaActual > 1) this.paginaActual--;
  }

  paginaSiguiente() {
    if (this.paginaActual < this.totalPaginas) this.paginaActual++;
  }

  irAPagina(pagina: number) {
    if (pagina >= 1 && pagina <= this.totalPaginas) this.paginaActual = pagina;
  }

  get paginasDisponibles(): number[] {
    const paginas: number[] = [];
    const maxBotones = 5;
    let inicio = Math.max(this.paginaActual - 2, 1);
    let fin = Math.min(inicio + maxBotones - 1, this.totalPaginas);

    if (fin - inicio < maxBotones - 1) {
      inicio = Math.max(fin - maxBotones + 1, 1);
    }

    for (let i = inicio; i <= fin; i++) {
      paginas.push(i);
    }

    return paginas;
  }

  nuevo() {
    this.formularioEnviado = false;
    this.personaSeleccionada = {
      id: 0,
      nombreCompleto: '',
      identificacion: '',
      edad: null,
      genero: '',
      estadoActivo: true,
      maneja: false,
      usaLentes: false,
      diabetico: false,
      enfermedadOtra: '',
    };
    this.errorMensaje = '';
    this.personaForm.reset({ ...this.personaSeleccionada });
    this.modalAbierto = true;
  }

  confirmarCerrarSesion: boolean = false;

  abrirConfirmarCerrarSesion() {
    this.confirmarCerrarSesion = true;
  }

  cancelarCerrarSesion() {
    this.confirmarCerrarSesion = false;
  }

  cerrarSesionConfirmado() {
    this.authService.logout();
    this.router.navigate(['/login']);
    this.confirmarCerrarSesion = false;
  }

  editar(persona: Persona) {
    this.formularioEnviado = false;
    this.personaSeleccionada = { ...persona };
    this.errorMensaje = '';
    this.personaForm.patchValue(this.personaSeleccionada);
    this.modalAbierto = true;
  }

  cancelar() {
    this.formularioEnviado = false;
    this.personaSeleccionada = null;
    this.personaForm.reset();
    this.errorMensaje = '';
    this.modalAbierto = false;
  }

  guardar() {
    this.formularioEnviado = true;

    if (!this.personaSeleccionada || this.personaForm.invalid) {
      this.errorMensaje =
        'Por favor complete todos los campos obligatorios correctamente.';
      return;
    }

    this.errorMensaje = '';

    const persona: Persona = {
      ...this.personaSeleccionada,
      ...this.personaForm.value,
    };

    if (persona.id && persona.id > 0) {
      this.api.actualizarPersona(persona.id, persona).subscribe({
        next: () => {
          this.cancelar();
          this.cargarPersonas();
        },
        error: (err) =>
          (this.errorMensaje = err.message || 'Error al actualizar persona'),
      });
    } else {
      this.api.crearPersona(persona).subscribe({
        next: () => {
          this.cancelar();
          this.cargarPersonas();
        },
        error: (err) =>
          (this.errorMensaje = err.message || 'Error al crear persona'),
      });
    }
  }

  borrar(id: number) {
    if (!this.esAdmin) return;
    if (!confirm('¿Seguro que quieres eliminar esta persona?')) return;

    this.api.eliminarPersona(id).subscribe({
      next: () => {
        this.personas = this.personas.filter((p) => p.id !== id);
        this.filtrarPersonas(false);
      },
      error: (err) =>
        (this.errorMensaje = err.message || 'Error al eliminar persona'),
    });
  }
}
