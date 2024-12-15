var megamenu = document.querySelector('.megamenu');
var megamenu_toggle = document.querySelector('.megamenu-toggle');
$(window).on('resize', function() {
    if (window.innerWidth < 960) {
        megamenu.classList.add('drop-down-none');
        megamenu_toggle.classList.remove('dropdown-toggle');
    } else {
        megamenu.classList.remove('drop-down-none');
        megamenu_toggle.classList.add('dropdown-toggle');
    }
});

const indexPage = document.querySelector('#index');
if (indexPage) {
    $('.section-3').on('inview', function(event, isInView) {
        if (isInView) {
            $('.catal-bicak').show();
            $('.catal-bicak').addClass("animate__fadeInDown animate__slow");
        }
    });
    $('.section-3').on('inview', function(event, isInView) {
        if (isInView) {
            $('.baton-simit-1').show();
            $('.baton-simit-1').addClass("animate__fadeInUp animate__slow");
        }
    });
    $('.section-3').on('inview', function(event, isInView) {
        if (isInView) {
            $('.metin-2').show();
            $('.metin-2').addClass("animate__fadeInUp animate__slow");
        }
    });

    const spaceHolder1 = document.querySelector('.space-holder');
    const horizontal1 = document.querySelector('.horizontal-section');
    const kup1 = document.querySelector('#slide-1 .kup');
    const kup2 = document.querySelector('#slide-2 .kup');
    const yaprak1 = document.querySelector('.yaprak1');
    const yaprak2 = document.querySelector('.yaprak2');
    spaceHolder1.style.height = `${calcDynamicHeight1(horizontal1)}px`;
    kup2.style.opacity = 0;

    function calcDynamicHeight1(ref) {
        const vw = window.innerWidth;
        const vh = window.innerHeight;
        const objectWidth = ref.scrollWidth;
        return objectWidth - vw + vh;
    }

    window.addEventListener('scroll', () => {
        const sticky1 = document.querySelector('.sticky');
        horizontal1.style.transform = `translateX(-${sticky1.offsetTop}px)`;
        var y = sticky1.offsetTop;
        kup2.style.opacity = y / 500;
        kup1.style.opacity = 200 / y;
        yaprak1.style.left = 10 + y * 100 / 665 + '%';
        yaprak2.style.left = (y * 100 / 665) + 70 + '%';
    });

    window.addEventListener('resize', () => {
        spaceHolder1.style.height = `${calcDynamicHeight1(horizontal1)}px`;
    });




    const spaceHolder = document.querySelector('.section-5 .space-holder');
    const horizontal = document.querySelector('.section-5 .horizontal');
    spaceHolder.style.height = `${calcDynamicHeight(horizontal)}px`;

    function calcDynamicHeight(ref) {
        const vw = window.innerWidth;
        const vh = window.innerHeight;
        const objectWidth = ref.scrollWidth;
        return objectWidth - vw + vh + 150;
    }
    window.addEventListener('scroll', () => {
        const sticky = document.querySelector('.section-5 .sticky');
        horizontal.style.transform = `translateX(-${sticky.offsetTop}px)`;
    });
    window.addEventListener('resize', () => {
        spaceHolder.style.height = `${calcDynamicHeight(horizontal)}px`;
    });

}
const productPage = document.querySelector('#products');
if (productPage) {
    var productDetails = function(urunID) {
        var pd = document.getElementById('pd' + urunID);
        var pt = document.getElementById('pt' + urunID);
        var pb = document.getElementById('pb' + urunID);
        var modalD = document.getElementById('modal-icerik');
        var modalT = document.getElementById('modal-tablo');
        var modalB = document.getElementById('modal-baslik');

        modalD.innerHTML = pd.value;
        modalT.innerHTML = pt.value;
        modalB.innerHTML = pb.innerHTML;
    }
}
const contactPage = document.querySelector('#contact');
if (contactPage) {

}