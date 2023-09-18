import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';

import { AppRoutingModule } from './app-routing.module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { LayoutModule } from './shared/layout/layout.module';
import { JwtInterceptorService } from './core/services/jwt-interceptor.service';
import { FeaturesModule } from './features/features.module';
import { FormlyModule } from '@ngx-formly/core';
import { ReactiveFormsModule } from '@angular/forms';
import { FormlyBootstrapModule } from '@ngx-formly/bootstrap';

@NgModule({
  declarations: [AppComponent],
  imports: [
    AppRoutingModule,
    LayoutModule,
    FeaturesModule,
    FormlyModule.forRoot(),
    ReactiveFormsModule,
    FormlyBootstrapModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptorService,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
