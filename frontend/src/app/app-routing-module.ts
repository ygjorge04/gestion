import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UsuariosComponent } from './features/usuarios/usuarios.component';
import { ReservasComponent } from './features/reservas/reservas.component';
import { EspaciosComponent } from './features/espacios/espacios.component';

const routes: Routes = [
  { path: 'usuarios', component: UsuariosComponent },
  { path: 'reservas', component: ReservasComponent },
  { path: 'espacios', component: EspaciosComponent },
  { path: '', redirectTo: '/usuarios', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
