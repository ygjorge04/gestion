import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing-module';
import { AppComponent } from './app.component';

import { UsuariosComponent } from './features/usuarios/usuarios.component';
import { ReservasComponent } from './features/reservas/reservas.component';
import { EspaciosComponent } from './features/espacios/espacios.component';

@NgModule({
  declarations: [
    AppComponent,
    UsuariosComponent,
    ReservasComponent,
    EspaciosComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
