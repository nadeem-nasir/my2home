import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { Router,ActivatedRoute } from '@angular/router';

import { AuthenticationService, NotificationService } from '../../../services';
import { Subject } from 'rxjs/Subject';
@Component({
  selector: 'nb-reset-password-page',
  styleUrls: ['./reset-password.component.scss'],
  templateUrl: './reset-password.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NbResetPasswordComponent  implements OnInit, OnDestroy 
{

  private ngUnsubscribe: Subject<void> = new Subject<void>();


  redirectDelay: number = 0;
  showMessages: any = {};
  strategy: string = '';
  submitted = false;
  errors: string[] = [];
  messages: string[] = [];
  user: any = {};
  loading: boolean = false;

  constructor(private authenticationService: AuthenticationService,
    private route: ActivatedRoute, private router: Router,
    private toastrService: NotificationService, )
  {
    
  }

  ngOnInit()
  {
    this.route.queryParams.subscribe(params =>
    {
      this.user.code = params['code'];
      this.user.userId = params['userId'];
    });


  }
  ngOnDestroy() {
    // If subscribed, we must unsubscribe before Angular destroys the component.
    // Failure to do so could create a memory leak.
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  resetPass(): void
  {

    this.loading = true;    
    this.authenticationService.resetpassword(this.user)
      .subscribe(data =>
      {
        this.toastrService.showSuccess('Password', 'Your data has been successfully changed');
        this.router.navigate(['../reset-password-confirmation'], { relativeTo: this.route });
        this.loading = false;
      }, error =>
      {
        this.loading = false;
        this.toastrService.showDanger("Password", <any>error)
       })

  }
}
