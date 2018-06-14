/**
 * Particleground demo
 * @author Jonathan Nicol - @mrjnicol
 */

document.addEventListener('DOMContentLoaded', function () {
   particleground(document.getElementById('main_content'), {
      dotColor: 'rgba(255, 255, 255, 0.5)',
      lineColor: 'rgba(255, 255, 255, 0.05)',
      minSpeedX: 0.075,
      maxSpeedX: 0.15,
      minSpeedY: 0.075,
      maxSpeedY: 0.15,
      density: 15000,
      curvedLines: false,
      proximity: 150,
      parallaxMultiplier: 20,
      particleRadius: 3
   });
   var intro = document.getElementById('intro');
   intro.style.marginTop = - intro.offsetHeight / 2 + 'px';
}, false);
