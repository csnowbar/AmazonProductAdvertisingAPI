function preloadPage(pageParameter) {
    var page;
    $.ajax({
        type: 'GET',
        url: '/Product/List',
        data: { "page": pageParameter },
        success: alert("success")
        
    });
};


//function preloadPage(pageParameter, callback) {
//    var page;
//    $.ajax({
//        type: 'GET',
//        url: '/Product/List',
//        data: { "page": pageParameter },
//        success: function (response) {
//            callback(response, pageParameter);
//        }
//    });
//};