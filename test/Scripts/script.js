function FillTable() {

    // собрать все данные из элементов формы
    var search = $('#searchOrder').val();
    var datefrom = $('#datefrom').val();
    var dateto = $('#dateto').val();
    var result = $('#resultDescKey').val();

    $.ajaxSetup({ cache: false });

    $('.table').children('tbody').empty();
    $('.table').children('tbody').append("<tr><td class=\"load\" colspan=\"8\">Подгружаем...</td></tr>");

    // сделать запрос
    $.getJSON("/api/Data", { search: search, datefrom: datefrom, dateto: dateto, result: result }, function (data) {
        // очищаем таблицу
        $('.table').children('tbody').empty();
        // наполняем
        $.each(data, function (i, item) {
            var tr = '<tr>';
            tr += '<td><span class=\"view\">' + item.ORDERID + '</span></td>';
            tr += '<td>' + item.PURCHASEAMT + '</td>';
            tr += '<td>' + item.ACCOUNT + '</td>';
            tr += '<td>' + item.RESULT + '</td>';
            tr += '<td>' + item.RESULTDESC + '</td>';
            tr += '<td>' + item.DATETIME + '</td>';
            tr += '<td>' + item.RRN + '</td>';
            tr += '<td>' + item.DEALID + '</td>';
            tr += '</tr>';
            $('.table').children('tbody').append(tr);
        });

    });
}

$(function () {

    FillTable();

    // выбор RESULT
    $('#resultDescKey').on('change', function () {
        FillTable();
    });

    // поиск
    $("#searchOrder").keypress(function () {
        FillTable();
    });

    $('input[type="date"]').change(function () {
        FillTable();
    });

    $('.table').on('click', '.view', function () {

        var id = $(this).text();

        $.ajaxSetup({ cache: false });

        $('.table1').children('tbody').empty();

        $.getJSON("/Home/GetDatails", { id: id }, function (data) {

            $.each(data, function (i, item) {
                var tr = '<tr>';
                tr += '<td><span class=\"view\">' + item.ORDERID + '</span></td>';
                tr += '<td>' + item.PURCHASEAMT + '</td>';
                tr += '<td>' + item.ACCOUNT + '</td>';
                tr += '<td>' + item.RESULT + '</td>';
                tr += '<td>' + item.RESULTDESC + '</td>';
                tr += '<td>' + item.DATETIME + '</td>';
                tr += '<td>' + item.RRN + '</td>';
                tr += '<td>' + item.DEALID + '</td>';
                tr += '</tr>';
                $('.table1').children('tbody').append(tr);
            });

        });

        $("#ViewDetail").modal("show");
    });

});