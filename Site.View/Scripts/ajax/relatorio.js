$(document).ready(function () {

    var host = "http://websistema.net.br/estoque";
    host = "";

    $(document).on('click', '#apenasSubtotal', function () {
        var subtotalheck = $("#apenasSubtotal:checked");
        if (subtotalheck.length == 0) {
            $(".mostrar").addClass("hidden");
            $(".esconder").removeClass("hidden");
        } else {
            $(".mostrar").removeClass("hidden");
            $(".esconder").addClass("hidden");
        }
    });

    $(document).on('click', '#buscarRelatorioFaccao', function () {

        var faccao = $("#faccao").val();
        var dataInicial = $("#dataInicial").val();
        var dataFinal = $("#dataFinal").val();
        var acao = $("#acao").val();
        var status = $("#status").val();

        if (faccao == 0) {
            jAlert("INFORME A FACÇÃO", 'Atenção');
            return false;
        }

        $.ajax({
            'url': host + '/Relatorio/RelatorioPorFaccao',
            'type': 'POST',
            'data': {
                'Faccao': faccao,
                'DataInicial': dataInicial,
                'DataFinal': dataFinal,
                'Acao': acao,
                'StatusFluxo': status
            },
            'success': function (data) {


                $("#result").html(data);


            }
        });
        




        
    });







});