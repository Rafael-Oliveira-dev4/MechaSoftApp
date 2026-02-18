import { NgModule } from '@angular/core';
import { provideClientHydration } from '@angular/platform-browser';
import { provideServerRendering, withRoutes } from '@angular/ssr';
import { App } from './app.component';
import { AppModule } from './app.module';
import { serverRoutes } from './app.routes.server';

@NgModule({
  imports: [AppModule],
  providers: [
    provideClientHydration(),
    provideServerRendering(withRoutes(serverRoutes)),
  ],
  bootstrap: [App],
})
export class AppServerModule {}
