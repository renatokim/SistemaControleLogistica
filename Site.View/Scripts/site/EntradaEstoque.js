$(document).ready(function () {

    $("#btn-atualizar").click(function () {
        var entradaSaida = $("#EntradaSaida").val();
        var tecidoAviamento = $("#TecidoAviamento").val();

        var tecidoIdTecido = $("#Tecido_IdTecido").val();
        var tecidoIdCor = $("#Tecido_IdCor").val();
        var tecidoIdCodigo = $("#Tecido_IdCodigo").val();
        var materiaPrimaId = $("#MateriaPrima_Id").val();
        var faccaoId = $("#Faccao_Id").val();
        
        if (tecidoAviamento == "T") {
            if (tecidoIdTecido == "" || tecidoIdCor == "" || tecidoIdCodigo == "") {
                jAlert('INFORME O TECIDO!', 'ATENÇÃO');
                return false;
            }
        }
        else if (tecidoAviamento == "A") {
            if (materiaPrimaId == "") {
                jAlert('INFORME O AVIAMENTO!', 'ATENÇÃO');
                return false;
            }

            if(entradaSaida == "S"){
                if (faccaoId == "") {
                    jAlert('INFORME A FACÇÃO!', 'ATENÇÃO');
                    return false;
                }
            }
        }
        return true;
    });

    $("#EntradaSaidaRelatorio").change(function () {
        var entradaSaida = $("#EntradaSaidaRelatorio").val();
        if (entradaSaida == 'E') {
            $(".faccaoRelatorio").hide();
        } else {
            $(".faccaoRelatorio").show();
        }
    });

    $(".botaoExcluir").click(function () {
        if (confirm("Confirma a exclusão?"))
            return true;
        else
            return false;
    });

    $(".incluirParte").click(function () {

        var erro = false;
        var categoriaId = $("#categoriaId").val();
        var unidadeMedida = $("#UnidadeMedida").val();
        var descricao = $("#Descricao").val();

        if (categoriaId == "") {
            $("#categoriaId").css("border-color", "#DF0101").css("border-width", "1px");
            erro = true;
        } else {
            $("#categoriaId").css("border-color", "").css("border-width", "");
        }

        if (unidadeMedida == "") {
            $("#UnidadeMedida").css("border-color", "#DF0101").css("border-width", "1px");
            erro = true;
        } else {
            $("#UnidadeMedida").css("border-color", "").css("border-width", "");
        }

        if (descricao == "") {
            $("#Descricao").css("border-color", "#DF0101").css("border-width", "1px");
            erro = true;
        } else {
            $("#Descricao").css("border-color", "").css("border-width", "");
        }

        if (erro) {
            return false;
        }

        var tipoAviamentos = new Array();

        $(".tipoAviamento").each(function (index) {
            var tipoAviamentosValor = Object();
            tipoAviamentosValor['Id'] = $(this).find('.id').text();
            tipoAviamentosValor['Qtde'] = $(this).find('.consumo').text();
            tipoAviamentosValor['Aviamento'] = $(this).find('.idAv').text();
            tipoAviamentos[index] = tipoAviamentosValor;
        });

        var aviamentoStr = JSON.stringify(tipoAviamentos);
        $("#tipoAviamentoJson").val(aviamentoStr);
        //return false;
    });

    $('body').on('click', '#adicionarTipoAviamento', function () {
        var tipoAviamento = $("#TipoAviamentoId option:selected");
        var consumoAviamento = $("#consumoAviamento");
        var tableTipoAviamentos = $("#tableTipoAviamentos");

        if (tipoAviamento.val() == 0 || tipoAviamento.val() == "") {
            tipoAviamento.parent().css("border-color", "red");
            return false;
        } else {
            tableTipoAviamentos.append("<tr class='tipoAviamento'><td class='aviamento'>" + tipoAviamento.text() + "</td><td class='consumo'>" + consumoAviamento.val() + "</td><td class='idT' style='display:none'></td><td class='idAv' style='display:none'>" + tipoAviamento.val() + "</td><td class='id' style='display:none'>0</td><td><a onclick='RemoveTipoAviamento(this)'>Remover</a></td></tr>");
            tipoAviamento.parent().css("border-color", "");
            tipoAviamento.parent().val("");
            tipoAviamento.val(0);
            consumoAviamento.val("");
        }

        return false;
    });
});

function RemoveTipoAviamento(tipo) {
    $(tipo).parent().parent().remove();
}