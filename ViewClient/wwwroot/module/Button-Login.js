document.addEventListener('DOMContentLoaded', function () {
    const profileButton = document.getElementById('profileButton');
    const profileDropdown = document.getElementById('profileDropdown');

    // Kiểm tra xem các phần tử có tồn tại không
    if (profileButton && profileDropdown) {
        profileButton.addEventListener('click', function () {
            profileDropdown.classList.toggle('show');
            this.setAttribute('aria-expanded', this.getAttribute('aria-expanded') === 'true' ? 'false' : 'true');
        });

        // Đóng dropdown khi click bên ngoài
        window.addEventListener('click', function (event) {
            if (!profileButton.contains(event.target) && !profileDropdown.contains(event.target)) {
                profileDropdown.classList.remove('show');
                profileButton.setAttribute('aria-expanded', 'false');
            }
        });
    } else {
        console.error('Các phần tử không tồn tại trong DOM.');
    }
});