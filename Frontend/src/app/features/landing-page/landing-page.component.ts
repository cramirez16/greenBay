import { Component, OnInit } from '@angular/core';
import { gsap } from 'gsap';

@Component({
  selector: 'app-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrls: ['./landing-page.component.css'],
})
export class LandingPageComponent implements OnInit {
  name = 'Angular';

  ngOnInit() {
    this.setupGsap();
  }

  setupGsap(): void {
    this.pageTransition();
    // this.contentAnimation();
  }

  pageTransition() {
    let tl = gsap.timeline();
    tl.to('ul.transition li', {
      duration: 2,
      scaleY: 0,
      transformOrigin: 'bottom left',
      stagger: 0.1,
      delay: 0.1,
    });
  }
}
