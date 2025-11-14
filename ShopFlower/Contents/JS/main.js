document.addEventListener('DOMContentLoaded', function () {
    // Handle click on the mobile menu button
    const btnMenuMobile = document.getElementById('btn-menu-mobile');
    if (btnMenuMobile) {
        btnMenuMobile.addEventListener('click', function () {
            document.querySelector('.header-menu').classList.add('current');
            document.querySelector('.mobile-nav-overflow').classList.toggle('open');
        });
    }

    // Handle click on the title menu to remove classes
    const titleMenu = document.querySelector('.header-menu .title_menu');
    if (titleMenu) {
        titleMenu.addEventListener('click', function () {
            document.querySelector('.header-menu').classList.remove('current');
            document.querySelector('.mobile-nav-overflow').classList.remove('open');
        });
    }

    // Handle click on the mobile-nav-overflow to toggle classes
    const mobileNavOverflow = document.querySelector('.mobile-nav-overflow');
    if (mobileNavOverflow) {
        mobileNavOverflow.addEventListener('click', function () {
            document.querySelector('.header-menu').classList.remove('current');
            this.classList.remove('open');
        });
    }
        
    // Handle dropdown menu toggle
    const dropdownToggles = document.querySelectorAll('#nav li > .open_mnu');
    dropdownToggles.forEach(function (element) {
        element.addEventListener('click', function (e) {
            e.preventDefault();

            const parentLi = this.closest('li');
            const dropdownMenu = Array.from(parentLi.children).find(child => child.classList.contains('dropdown-menu'));

            if (dropdownMenu) {
                dropdownMenu.style.display = dropdownMenu.style.display === 'block' ? 'none' : 'block';
                parentLi.classList.toggle('current');
                dropdownMenu.classList.toggle('current');
            }

            this.classList.toggle('current');
        });
    });

    // Handle footer section toggle
    const footerHeaders = document.querySelectorAll('.footer-click h4');

    footerHeaders.forEach(function (header) {
        header.addEventListener('click', function () {
            // Toggle class 'cls_mn' cho chính thẻ <h4> được click
            this.classList.toggle('cls_mn');

            // Tìm phần tử tiếp theo (next sibling) và toggle hiển thị
            const nextElement = this.nextElementSibling;
            if (nextElement) {
                // Toggle hiển thị bằng cách thay đổi style
                if (nextElement.style.display === 'none' || nextElement.style.display === '') {
                    nextElement.style.display = 'block';
                } else {
                    nextElement.style.display = 'none';
                }

                // Toggle class 'current' cho <ul> tiếp theo
                if (nextElement.tagName === 'UL') {
                    nextElement.classList.toggle('current');
                }
            }
        });
    });
});