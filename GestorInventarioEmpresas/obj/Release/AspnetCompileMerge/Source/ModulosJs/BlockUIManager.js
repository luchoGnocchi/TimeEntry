var BlockUIManager = (function ($) {    
    function isUIBlocked(boxSelector) {
        var $overlay = $(boxSelector).find('.overlay');
        return $(boxSelector).find('.overlay').length > 0 && !$overlay.hasClass('hidden');
    }

    function checkPendingRequest(boxSelector) {
        if ($.active > 0) {
            window.setTimeout(function () {
                checkPendingRequest(boxSelector)
            }, 500);
        }
        else {
            unblockBox(boxSelector);
        }
    }

    function unblockBox(boxSelector) {
        if (isUIBlocked(boxSelector)) {
            if ($(boxSelector).length > 0) {
                var $overlay = $(boxSelector).find('.overlay');
                if ($overlay.length > 0) {
                    $overlay.addClass('hidden');
                }
            }
        }
    }

    return {
        block: function (boxSelector/*, showProccesing, proccesingMsg*/) {
            //if (!isUIBlocked(boxSelector)) {
            //    if ($(boxSelector).length > 0) {
            //        var $overlay = $(boxSelector).find('.overlay');
            //        var proccesingElement = '';
            //        showProccesing = showProccesing || false;
            //        if (showProccesing) {
            //            proccesingMsg = proccesingMsg || "Procesando...";
            //        } else {
            //            proccesingMsg = '';
            //        }
            //        if ($overlay.length == 0) {
            //            proccesingElement = '<div class="overlay-proccesing-msg">' + proccesingMsg + '</div>';
            //            $('<div class="overlay hidden"><i class="fa fa-refresh fa-spin"></i>' + proccesingElement + '</div>').insertAfter($(boxSelector).find('.box-body'));
            //            $overlay = $(boxSelector).find('.overlay');
            //        }
            //        if (showProccesing) {
            //            var $msg = $overlay.find('.overlay-proccesing-msg');
            //            if ($msg.length > 0) {
            //                $msg.html(proccesingMsg);
            //            }
            //        }
            //        $overlay.removeClass('hidden');
            //    }
            //}  
            var settings = {
                message: "Procesando...",
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#000',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                }
            };
            if (boxSelector) {
                $(boxSelector).block(settings);
            } else {
                $.blockUI(settings);
            }
            
        },
        unblock: function (boxSelector/*, checkAjaxPendingRequest*/) {
            //checkAjaxPendingRequest = checkAjaxPendingRequest || false;
            //if (checkAjaxPendingRequest)
            //    checkPendingRequest(boxSelector);
            //else
            //    unblockBox(boxSelector);
            if (boxSelector)
                $(boxSelector).unblock();
            else
                $.unblockUI();
        }
    }
})(jQuery);
