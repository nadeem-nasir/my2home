import { Component } from '@angular/core';

@Component({
  selector: 'ngx-footer',
  styleUrls: ['./footer.component.scss'],
  template: `
    <span class="created-by">Created with ♥ by <b><a href="http://technozm.com/" target="_blank">ZM Technologies</a></b> 2019</span>
    <div class="socials">     
      <a href="https://www.facebook.com/ZMTechnologiesOfficial/" target="_blank" class="ion ion-social-facebook"></a>     
    </div>
  `,
})
export class FooterComponent {
}
