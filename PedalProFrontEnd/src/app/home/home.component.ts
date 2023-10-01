import { animate, style, transition, trigger } from '@angular/animations';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ScrollingService } from '../Services/scrolling.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  animations: [ trigger('floatUp', [ transition(':enter', 
              [style({ transform: 'translateY(100%)', opacity: 0 }),
              animate('0.6s ease-out', style({ transform: 'translateY(0)', opacity: 1 }))
            ]) ])
          ]
})
export class HomeComponent implements OnInit{
  
  constructor() { }

  ngOnInit(): void { }

  scrollTo(targetId: string) {
    const targetElement = document.getElementById(targetId);
    if (targetElement) {
      window.scrollTo({
        top: targetElement.offsetTop,
        behavior: 'smooth'
      });
    }
  }

  
  
}
