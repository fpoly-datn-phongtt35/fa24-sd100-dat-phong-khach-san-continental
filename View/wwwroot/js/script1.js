'use strict';

'use strict';

document.addEventListener('DOMContentLoaded', function () {
    const selectElement = document.querySelector('.form-select1');

    selectElement.addEventListener('click', function (event) {
        event.stopPropagation();
        this.classList.toggle('open');
    });

    selectElement.addEventListener('change', function () {
        this.classList.remove('open');
    });

    document.addEventListener('click', function (event) {
        if (!selectElement.contains(event.target)) {
            selectElement.classList.remove('open');
        }
    });

    const options = selectElement.querySelectorAll('option');
    if (options.length > 6) {
        selectElement.style.maxHeight = '12rem';
        selectElement.style.overflowY = 'auto';
    }
});




