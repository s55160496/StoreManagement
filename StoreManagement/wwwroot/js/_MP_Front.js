$(function () {
    var $body = $('body');

    var $MENU_SIDE = $('#MENU_SIDE >.menu');
    var $SITE_CONTENT = $('#SITE_CONTENT');
    var $CONTENT_OVERLAY = $('#CONTENT_OVERLAY');
    var $MENU_TRIGGER = $('#MENU_TRIGGER');

    var class_SmallHEADER = 'header-sm';
    var class_SideNAVBAR = 'navbar-side';

    var MenuSide = {
        nLevelCurrent: 0,
        Next: function () {
            this.nLevelCurrent += 1;
            this.TranslateX();
        },
        Previous: function (isToFirst) {
            isToFirst = Boolean(isToFirst);
            this.nLevelCurrent -= (isToFirst ? this.nLevelCurrent : 1);
            this.TranslateX();
            if (isToFirst) $MENU_SIDE.find('a.menu-view').removeClass('menu-view');
        },
        TranslateX: function () {
            var nRangeTrans = this.nLevelCurrent * -100; //Percentage (%)
            $MENU_SIDE.css('-webkit-transform', 'translateX(' + nRangeTrans + '%)');
            $MENU_SIDE.css('transform', 'translateX(' + nRangeTrans + '%)');
        }
    };
    $MENU_SIDE
        .delegate('li.has-children > a', 'click', function (e) {
            $(this).addClass('menu-view');
            MenuSide.Next();
            e.preventDefault();
        })
        .delegate('a.link-back', 'click', function (e) {
            MenuSide.Previous();
            $(this).parents('ul.menu-sub:first').prev('a').removeClass('menu-view');
            e.preventDefault();
        });
    $MENU_TRIGGER.on('click', function (e) { $body.addClass(class_SideNAVBAR); e.preventDefault(); });
    $CONTENT_OVERLAY.on('click', function (e) { $body.removeClass(class_SideNAVBAR); MenuSide.Previous(true); e.preventDefault(); });


    var $boxCover = $('#boxCover');
    var $boxWelcome = $('#boxWelcome');

    var boxWelcome_Preparing = function () {
        setTimeout(function () {
            $boxWelcome.css('left', '');
            $boxWelcome.css('right', '');

            var nHSpace = $boxCover.position().left;
            $boxWelcome.css('left', '-' + nHSpace + 'px');
            $boxWelcome.css('right', '-' + nHSpace + 'px');
        }, 100);
    };

    window.addEventListener('resize', boxWelcome_Preparing);
    boxWelcome_Preparing();

    function toggleDropdown(e) {
        const _d = $(e.target).closest('.dropdown'),
          _m = $('.dropdown-menu', _d);
        setTimeout(function () {
            const shouldOpen = e.type !== 'click' && _d.is(':hover');
            _m.toggleClass('show', shouldOpen);
            _d.toggleClass('show', shouldOpen);
            $('[data-toggle="dropdown"]', _d).attr('aria-expanded', shouldOpen);
        }, e.type === 'mouseleave' ? 300 : 0);
    }

    $('.menu')
      .on('mouseenter mouseleave', '.dropdown', toggleDropdown)
      .on('click', '.dropdown-menu a', toggleDropdown);
});