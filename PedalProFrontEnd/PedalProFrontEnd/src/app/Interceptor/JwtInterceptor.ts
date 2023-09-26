import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent,HttpResponse, HttpErrorResponse  } from '@angular/common/http';
import { Observable,throwError  } from 'rxjs';
import { Router } from '@angular/router';
import { catchError, tap } from 'rxjs/operators';
import { PedalProServiceService } from '../Services/pedal-pro-service.service';
import { MatDialog } from '@angular/material/dialog';
import { ErrorDialogComponent } from 'src/app/Dialogs/error-dialog/error-dialog.component';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  
  constructor(private router: Router,private service:PedalProServiceService,private dialog:MatDialog){}

  Logout()
  {
    
    this.service.logouttwo();
  }

  openErrorDialog(errorMessage: string): void {
    this.dialog.open(ErrorDialogComponent, {
      data: { message: errorMessage }
    });
  }
  
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Retrieve the JWT from local storage
    const jwt = localStorage.getItem('jwt');

    // If the JWT is available, add it to the request header
    if (jwt) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${jwt}`
        }
      });
    }

    // Continue with the modified request
    return next.handle(request).pipe(
      tap((event: HttpEvent<any>) => {
        // Handle successful responses here if needed
        if (event instanceof HttpResponse) {
          // For example, you can check for certain response headers or status codes.
        }
      }),
      catchError((error: HttpErrorResponse) => {
        if (error.status === 0) {
          // Server connection lost
          // Display a snackbar message
          
          
          /*this.snackBar.open('Connection to the server was lost.', 'Close', {
            duration: 5000, // Display for 5 seconds
          });*/

          // Perform logout and redirect to a login page
          this.Logout();
        }

        // Continue to propagate the error
        return throwError(error);
      })
    );
  }
}