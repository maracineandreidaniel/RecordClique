$(document).ready(function () {
    $('.slider').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 2000,
        nextArrow: '<button type="button" class="slick-next">Next</button>',
        prevArrow: '<button type="button" class="slick-prev">Previous</button>',
        centerMode: true,
        variableWidth: true
    });
});
