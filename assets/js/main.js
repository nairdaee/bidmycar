(function () {

	"use strict";

	//===== Prealoder

	window.onload = function () {
		window.setTimeout(fadeout, 200);

        let body = document.body;

        if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
                // dark mode
                // body.classList.remove("theme--default");
                // body.classList.add("theme--dark");
        }
	}


    function fadeout() {
        document.querySelector('.preloader').style.opacity = '0';
        document.querySelector('.preloader').style.display = 'none';
    }


    /*=====================================
    Sticky
    ======================================= */
    window.onscroll = function () {
        var header_navbar = document.querySelector(".navbar-area");
        var sticky = header_navbar.offsetTop;

        if (window.pageYOffset > sticky) {
            header_navbar.classList.add("sticky");
        } else {
            header_navbar.classList.remove("sticky");
        }

        // show or hide the back-top-top button
        var backToTo = document.querySelector(".scroll-top");
        if (document.body.scrollTop > 50 || document.documentElement.scrollTop > 50) {
            backToTo.style.display = "flex";
        } else {
            backToTo.style.display = "none";
        }
    };

    //===== Mobile-menu-btn
	let navbarToggler = document.querySelector(".mobile-menu-btn");
	navbarToggler.addEventListener('click', function () {
		navbarToggler.classList.toggle("active");
	});
    
    // WOW active
    new WOW().init();

    function themeToggle() {
        var body = document.body;

        if (body.classList.contains("theme--default")) {
            body.classList.remove("theme--default");
            body.classList.add("theme--dark");

        } 
        else if (body.classList.contains("theme--dark")) {
            body.classList.remove("theme--dark");
            body.classList.add("theme--default");
        }
    }

    

            
    
})();

