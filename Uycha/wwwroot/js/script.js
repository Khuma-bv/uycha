
  
// Replace Text In Header
const checkReplace = document.querySelector('#replace-me');

if (checkReplace !== null) {
  const replace = new ReplaceMe(checkReplace, {
    animation: 'animated fadeIn',
    speed: 2000,
    separator: ',',
    loopCount: 'infinite',
    autoRun: true,
  });
}

// User Scroll For Navbar
function userScroll() {
  const navbar = document.querySelector('.navbar');

  window.addEventListener('scroll', () => {
    if (window.scrollY > 50) {
      navbar.classList.add('bg-light');
      navbar.classList.add('border-bottom');
      navbar.classList.add('border-secondary');
      navbar.classList.add('navbar-sticky');
      navbar.classList.add('opacity-75');
    } else {
      navbar.classList.remove('bg-dark');
      navbar.classList.remove('border-bottom');
      navbar.classList.remove('border-secondary');
      navbar.classList.remove('navbar-sticky');
    }
  });
}

document.addEventListener('DOMContentLoaded', userScroll);


// document.addEventListener("DOMContentLoaded", function () {
//     var carousels = document.querySelectorAll(".carousel"); // Находим все карусели
//     carousels.forEach(function (carouselElement) {
//       new bootstrap.Carousel(carouselElement, {
//         interval: 8000 
//       });
//     });
//   });