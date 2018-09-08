var host = "http://websistema.net.br/estoque";
var hostBuscarPedido = "/estoque/pedido/BuscarProdutoPedido";

host = "";
var hostBuscarPedido = "/pedido/BuscarProdutoPedido";

$(document).on('click', '.btnPaginacao', function () {
    $("#pleaseWaitDialog").modal('show');
    var pagina = $(this).attr('data-value') -1;
    var filtro = $("#filtroHide").text();
    
    var url = host + "/Fluxo/Paginar?filtro=" + filtro + "&pagina=" + pagina;
    $(location).attr("href", url);
});

$(document).on('click', '#buscarPaginacaoFluxo', function () {
    $("#pleaseWaitDialog").modal('show');
});

$(document).on('click', '.loading', function () {
    $("#pleaseWaitDialog").modal('show');
});

$(document).on('click', '.finalizar', function () {
    if (confirm("Finalizar o Pedido?"))
        $("#pleaseWaitDialog").modal('show');
    else
        return false;
    return true;
});

$(document).on('click', '.obs', function () {
    var texto = $(this).find('div').text().trim();
    var id = $(this).attr('id');

    jPrompt('Obs dia:', texto, 'Obs', function (r) {
        if (r) {
            var obs = r;
            $.ajax({
                'url': host + '/Pedido/AlterarObs',
                'type': 'POST',
                'data': {
                    'id': id,
                    'texto': obs
                },
                'success': function (data) {
                    if (data) {
                        $('#' + id).html(data);
                    }
                }
            });
        };
    });
});

$(document).ready(function () {
    var dataInicial = $("#dataInicial").val();
    var dataFinal = $("#dataFinal").val();

    if (dataInicial == "") {
        jAlert("INFORME A DATA INICIAL", 'Atenção');
        $("#dataFinal").val('');
        return false;
    }

    $.ajax({
        'url': host + '/Pedido/PedidosRelatorio',
        'type': 'POST',
        'data': {
            'dataInicial': dataInicial,
            'dataFinal': dataFinal
        },
        'success': function (data) {
            if (data) {
                $("#tabelaPedidos").html(data);
            }
        }
    });

});

$(document).on('change', '#dataFinal', function () {
    var dataInicial = $("#dataInicial").val();
    var dataFinal = $("#dataFinal").val();

    if (dataInicial == "") {
        jAlert("INFORME A DATA INICIAL", 'Atenção'); 
        $("#dataFinal").val('');
        return false;
    }

    $.ajax({
        'url': host + '/Pedido/PedidosRelatorio',
        'type': 'POST',
        'data': {
            'dataInicial': dataInicial,
            'dataFinal': dataFinal
        },
        'success': function (data) {
            if (data) {
                $("#tabelaPedidos").html(data);
            }
        }
    });
});

var pedidosCheck = $(".pedidos_check:checked");

$(document).on('click', '#btnRelatorioConsumo', function () {
    var pedidosCheck = $(".checkPedidos:checked");

    pedidosCheck.each(function (index) {
        $(this).attr('name', 'IdPedidos[' + index + ']');
    });
});

$(document).on('change', '#ClienteId', function () {
    var idCliente = $("#ClienteId").val();
    $.ajax({
        'url': host + '/Cliente/ClientePorId',
        'type': 'POST',
        'data': {
            'id': idCliente
        },
        'success': function (data) {
            if (data) {
                $("#VendedorId").html(data);
            }
        }
    });
});

function mostrarItens() {
    $("#Modal_ItensPesquisa").modal('show');
}

$(document).on('click', '.btn_incluir', function () {
    var id = $(this).attr("data-id");
    var pedidoAtual = $("#pedidoAtual").val();
    
    $.ajax({
        'url': host + '/Pedido/IncluirProdutoPesquisa',
        'type': 'POST',
        'data': {
            'id': id,
            'pedidoAtual': pedidoAtual
        },
        'success': function (data) {
            if (data) {
                $("#pedidoAtual").val(data);
                $("#pedidoAtualPesquisa").val(data);
            }
        }
    });
});

$("#buscarProduto").click(function () {
    needToConfirm = false;
    var pedidoAtual = $("#pedidoAtual").text();
    $("#pedidoJson").val(pedidoAtual);
    $("form").attr("action", hostBuscarPedido);
    $("form").submit();

    return false;
});

$('body').on('click', '.verProdutoPesquisa', function () {
    var linha = $(this);
    var id = linha.attr("data-produto");

    $.ajax({
        'url': host + '/Pedido/ProdutosPorPedidos',
        'type': 'POST',
        'data': {
            'id': id
        },
        'success': function (data) {
            if (data) {
                $("#Modal_ProdutosPesquisa .modal-body").html(data);
                $("#Modal_ProdutosPesquisa").modal('show');
            }
        }
    });
});

$("#PedidoProdutoPeca").change(function () {
    $.ajax({
        'url': host + '/Produto/ModeloProdutoPorPeca',
        'type': 'GET',
        'data': {
            'peca': $("#PedidoProdutoPeca").val()
        },
        'success': function (data) {
            if (data) {
                $("#PedidoProdutoModelo").html(data);
            }
        }
    });
});

$("#PedidoProdutoTecido").change(function () {
    $.ajax({
        'url': host + '/Tecido/CorPorTipoTecido',
        'type': 'GET',
        'data': {
            'idTipoTecido': $("#PedidoProdutoTecido").val()
        },
        'success': function (data) {
            if (data) {
                $("#PedidoProdutoTecidoCor").html(data);
                $("#PedidoProdutoTecidoCodigo").html("<option value='0'></option>");
            }
        }
    });
});

$("#PedidoProdutoTecidoCor").change(function () {
    $.ajax({
        'url': host + '/Tecido/CodigoPorTipoTecidoCor',
        'type': 'GET',
        'data': {
            'idTipoTecido': $("#PedidoProdutoTecido").val(),
            'idCor': $("#PedidoProdutoTecidoCor").val()
        },
        'success': function (data) {
            if (data) {
                $("#PedidoProdutoTecidoCodigo").html(data);
            }
        }
    });
});

$(".botaoPartes").click(function () {

    $("#Modal_Partes #quantidadeItemPeca").val('0');
    $("#Modal_Partes #ParteCategoria").val('0');
    $("#Modal_Partes #obsItemPeca").val('');
    $("#Modal_Partes #quantidadeAviamento").val('');
    $("#Modal_Partes #idTipoAviamento").html("<option value='0'>Tipo Aviamento</option>");
    $("#Modal_Partes #idCorAviamento").html("<option value='0'>Cor Aviamento</option>");
    $("#Modal_Partes #idCodigoAviamento").html("<option value='0'>Codigo Aviamento</option>");
    $("#checkboxParte").prop("checked", true);
    $("#Modal_Partes #tecidoItem").val('0');
    $("#Modal_Partes #corItem").val('0');
    $("#Modal_Partes #codigoItem").val('0');
    $("#tableItens").html('');

    var botao = $(this).context;
    var labelModal = botao.innerHTML;
    var idCategoria = botao.getAttribute("data-id-categoria");

    $("#Modal_Partes #myModalLabel").text(labelModal);
    $("#Modal_Partes #ParteOuAcrescimo").val("Parte");

    $.ajax({
        'url': host + '/ParteAcrescimo/PartePorCategoriaId',
        'type': 'GET',
        'data': {
            'idCategoria': idCategoria
        },
        'success': function (data) {
            if (data) {
                $("#Modal_Partes #ParteCategoria").html(data);
            }
        }
    });
});

$(".botaoAcrescimos").click(function () {

    $("#Modal_Partes #quantidadeItemPeca").val('0');
    $("#Modal_Partes #ParteCategoria").val('0');
    $("#Modal_Partes #obsItemPeca").val('');
    $("#Modal_Partes #quantidadeAviamento").val('');
    $("#Modal_Partes #idTipoAviamento").html("<option value='0'>Tipo Aviamento</option>");
    $("#Modal_Partes #idCorAviamento").html("<option value='0'>Cor Aviamento</option>");
    $("#Modal_Partes #idCodigoAviamento").html("<option value='0'>Codigo Aviamento</option>");
    $("#checkboxParte").prop("checked", true);
    $("#Modal_Partes #tecidoItem").val('0');
    $("#Modal_Partes #corItem").val('0');
    $("#Modal_Partes #codigoItem").val('0');
    $("#tableItens").html('');

    var botao = $(this).context;
    var labelModal = botao.innerHTML;
    var idCategoria = botao.getAttribute("data-id-categoria");

    $("#Modal_Partes #myModalLabel").text(labelModal);
    $("#Modal_Partes #ParteOuAcrescimo").val("Acrescimo");

    $.ajax({
        'url': host + '/ParteAcrescimo/AcrescimoPorCategoriaId',
        'type': 'GET',
        'data': {
            'idCategoria': idCategoria
        },
        'success': function (data) {
            if (data) {
                $("#Modal_Partes #ParteCategoria").html(data);
            }
        }
    });
});

$("#Modal_Partes #ParteCategoria").change(function () {

    var idAviamento = $("#Modal_Partes #ParteCategoria").val();

    $.ajax({
        'url': host + '/ParteAcrescimo/TiposAviamentos',
        'type': 'GET',
        'data': {
            'idAviamento': idAviamento
        },
        'success': function (data) {
            if (data) {
                $("#Modal_Partes #idTipoAviamento").html(data);
                $("#Modal_Partes #idCorAviamento").html("<option value='0'>Cor Aviamento</option>");
                $("#Modal_Partes #idCodigoAviamento").html("<option value='0'>Codigo Aviamento</option>");
                $("#Modal_Partes #adicionarAviamento").addClass("hidden");
            }
        }
    });
});

$("#Modal_Partes #idTipoAviamento").change(function () {

    var idTipoAviamento = $("#Modal_Partes #idTipoAviamento").val();

    $.ajax({
        'url': host + '/Aviamento/CorPorTipoAviamento',
        'type': 'GET',
        'data': {
            'idTipoAviamento': idTipoAviamento
        },
        'success': function (data) {
            if (data) {
                $("#Modal_Partes #idCorAviamento").html(data);
                $("#Modal_Partes #idCodigoAviamento").html("<option value='0'>Codigo Aviamento</option>");
                $("#Modal_Partes #adicionarAviamento").addClass("hidden");
            }
        }
    });
});

$("#Modal_Partes #idCorAviamento").change(function () {

    var idTipoAviamento = $("#Modal_Partes #idTipoAviamento").val();
    var idCorAviamento = $("#Modal_Partes #idCorAviamento").val();

    $.ajax({
        'url': host + '/Aviamento/CodigoPorTipoAviamento',
        'type': 'GET',
        'data': {
            'idTipoAviamento': idTipoAviamento,
            'idCorAviamento': idCorAviamento
        },
        'success': function (data) {
            if (data) {
                $("#Modal_Partes #idCodigoAviamento").html(data);
                $("#Modal_Partes #adicionarAviamento").addClass("hidden");
            }
        }
    });
});

$('#checkboxParte').click(function () {
    if (!$(this).is(':checked')) {

        $.ajax({
            'url': host + '/Tecido/GetTecidosForModal',
            'type': 'GET',
            'success': function (data) {
                if (data) {
                    $("#Modal_Partes #tecidoItem").html(data);
                    $("#Modal_Partes #corItem").html("<option value='0'></option>");
                    $("#Modal_Partes #codigoItem").html("<option value='0'></option>");
                }
            }
        });

        $('#trMesmoTecido').removeClass("hidden");
    }
    else {
        $('#trMesmoTecido').addClass("hidden");
    }
});

$("#Modal_Partes #tecidoItem").change(function () {

    var idTecido = $("#Modal_Partes #tecidoItem").val();

    $.ajax({
        'url': host + '/Tecido/CorPorTipoTecido',
        'data': {
            'idTipoTecido': idTecido
        },
        'type': 'GET',
        'success': function (data) {
            if (data) {
                $("#Modal_Partes #corItem").html(data);
                $("#Modal_Partes #codigoItem").html("<option value='0'></option>");
            }
        }
    });

});

$("#Modal_Partes #corItem").change(function () {

    var idTecido = $("#Modal_Partes #tecidoItem").val();
    var idCor = $("#Modal_Partes #corItem").val();

    $.ajax({
        'url': host + '/Tecido/CodigoPorTipoTecidoCor',
        'type': 'GET',
        'data': {
            'idTipoTecido': idTecido,
            'idCor': idCor
        },
        'success': function (data) {
            if (data) {
                $("#Modal_Partes #codigoItem").html(data);
            }
        }
    });
});

$("#Modal_Partes #idCodigoAviamento").change(function () {

    var idCodigoAviamento = $("#Modal_Partes #idCodigoAviamento").val();

    if (idCodigoAviamento == 0) {
        $("#Modal_Partes #adicionarAviamento").addClass("hidden");
    } else {
        $("#Modal_Partes #adicionarAviamento").removeClass("hidden");
    }


});

function RemoveAviamento(rowAviamento) {
    $(rowAviamento).parent().parent().remove();
};

$("#SelectTamanhos").change(function () {
    var selectTamanhos = $("#SelectTamanhos option:selected").text();

    if (selectTamanhos == 0) {
        $("#SelectTamanhos").css("border-color", "red");
    } else {
        $("#SelectTamanhos").css("border-color", "");
    }
});

$("#adicionarTamanho").click(function () {
    var selectTamanhos = $("#SelectTamanhos option:selected").text();

    if (selectTamanhos == 0) {
        $("#SelectTamanhos").css("border-color", "red");
        return false;
    } else {
        var qtdeTamanho = $("#qtdeTamanho").val();
        if (qtdeTamanho == "") {
            qtdeTamanho = 0;
        }

        $("#tableTamanhos").append("<tr class='tamanhos'><td class='qtdeT'>" + qtdeTamanho + "</td><td class='valorT'>" + $("#SelectTamanhos option:selected").text() + "</td><td class='idT' style='display:none'>" + $("#SelectTamanhos").val() + "</td><td class='obsT'>" + $("#obsTamanho").val() + "</td><td><a onclick='RemoveTamanho(this)'>Remover</a></td></tr>");
        $("#qtdeTamanho").val('');
        $("#SelectTamanhos").val(0);
        $("#obsTamanho").val('');
    }

    //var tabela = $("#tableTamanhos tbody");

    return false;
});

$("#adicionarAviamento").click(function () {

    $("#tableItens").append("<tr><td>" +
        "<input name='QuantidadeAviamento' type='hidden' value='" + $("#quantidadeAviamento").val() + "' /><input name='TipoAviamento' type='hidden' value='" + $("#idTipoAviamento").val() + "' /><input name='CorAviamento' type='hidden' value='" + $("#idCorAviamento").val() + "' /><input name='CodigoAviamento' type='hidden' value='" + $("#idCodigoAviamento").val() + "' />"
        + $("#quantidadeAviamento").val() + " " + $("#idTipoAviamento option:selected").text() + " " + $("#idCorAviamento option:selected").text() + " " + $("#idCodigoAviamento option:selected").text() + "</td><td><a onclick='RemoveAviamento(this)'>Remover</a></td></tr>");
    $("#tableItens").removeClass("hidden");


    return false;
});

$("#adicionarPartes").click(function () {
    var aviamentos = new Array();

    var rowNumRemover = $("#parteAcrescimoEdit").val();

    var quantidadeItemPeca = $("#quantidadeItemPeca").val();
    var parteCategoria = $("#ParteCategoria").val();
    var obsItemPeca = $("#obsItemPeca").val();
    var parteOuAcrescimo = $("#ParteOuAcrescimo").val();

    var pedidoProdutoAtual = $("#ProdutoPedido").text();

    if (parteCategoria == 0) {
        $("#ParteCategoria").css("border-color", "red");
        return false;
    } else {
        $("#ParteCategoria").css("border-color", "");
    }

    /*if (quantidadeItemPeca == 0) {
        $("#quantidadeItemPeca").css("border-color", "red");
        return false;
    } else {
        $("#quantidadeItemPeca").css("border-color", "");
    }*/

    var checkboxParte = $("#checkboxParte:checked").length;
    var mesmoTecido = $("#checkboxParte").is(':checked');

    var tecidoItem = $("#tecidoItem").val();
    var corItem = $("#corItem").val();
    var codigoItem = $("#codigoItem").val();
    $('#trMesmoTecido').addClass("hidden");

    if (checkboxParte == 0) {
        if (tecidoItem == 0 || corItem == 0 || codigoItem == 0) {
            $("#tecidoItem").css("border-color", "red");
            $("#corItem").css("border-color", "red");
            $("#codigoItem").css("border-color", "red");
            return false;
        } else {
            $("#tecidoItem").css("border-color", "");
            $("#corItem").css("border-color", "");
            $("#codigoItem").css("border-color", "");
        }
    };

    $("#tableItens tbody tr").each(function (index) {
        var item = this;

        var quantidadeAviamento = $(item).find('td input[name="QuantidadeAviamento"]').val();
        var tipoAviamento = $(item).find('td input[name="TipoAviamento"]');
        var corAviamento = $(item).find('td input[name="CorAviamento"]');
        var codigoAviamento = $(item).find('td input[name="CodigoAviamento"]');

        if (quantidadeAviamento.trim() == "") {
            quantidadeAviamento = 0;
        };

        var aviamento = {
            "QuantidadeAviamento": quantidadeAviamento,
            "TipoAviamento": tipoAviamento.val(),
            "CorAviamento": corAviamento.val(),
            "CodigoAviamento": codigoAviamento.val()
        };

        aviamentos[index] = aviamento;
    });

    if (mesmoTecido) {
        tecidoItem = 0;
        corItem = 0;
        codigoItem = 0;
    };

    var tecido = {
        "TecidoItem": tecidoItem,
        "CorItem": corItem,
        "CodigoItem": codigoItem
    };

    var parteAcrescimo = {
        "Quantidade": quantidadeItemPeca,
        "ParteAcrescimo": parteCategoria,
        "Obs": obsItemPeca,
        "Aviamentos": aviamentos,
        "UsaMesmoTecido": mesmoTecido,
        "Tecido": tecido
    };

    $.ajax({
        'url': host + '/ParteAcrescimo/AdicionarParteAcrescimo',
        'type': 'POST',
        'data': {
            'parteAcrescimo': JSON.stringify(parteAcrescimo),
            'ParteOuAcrescimo': parteOuAcrescimo,
            'pedidoProdutoAtual': pedidoProdutoAtual,
            'rowNumRemover': rowNumRemover
        },
        'success': function (data) {
            if (data) {
                $("#collapseOne .accordion-inner").html(data);
                $("#Modal_Partes").modal('hide');
                $("#parteAcrescimoEdit").val(0);
            }
        }
    });

    return false;
});

$("#BtnIncluirProduto").click(function () {
    var qtdeProdutos = $("#qtdeProduto").val();
    var qtdeTamanhos = 0;
    $(".qtdeT").each(function () {
        qtdeTamanhos += parseInt($(this).text());
    });

    if (qtdeProdutos != qtdeTamanhos) {
        jAlert('QUANTIDADE DOS TAMANHOS NÃO CONFERE COM A QUANTIDADE DO PRODUTO!', 'Atenção');
        return false;
    }

    var fornecedores = new Array();
    var tamanhos = new Array();

    $(".tamanhos").each(function (index) {
        var tamanhoValor = Object();
        tamanhoValor['Id'] = $(this).find('.idT').text();
        tamanhoValor['Qtde'] = $(this).find('.qtdeT').text();
        tamanhoValor['Tamanho'] = $(this).find('.valorT').text();
        tamanhoValor['Obs'] = $(this).find('.obsT').text();
        tamanhos[index] = tamanhoValor;
    });

    var checkBoxFornecedores = $(".checkbox:checked");
    checkBoxFornecedores.each(function (i) {
        fornecedores[i] = this.value;
    });

    var qtdeProduto = $("#qtdeProduto");
    var pedidoProdutoPeca = $("#PedidoProdutoPeca");
    var pedidoProdutoModelo = $("#PedidoProdutoModelo");
    var pedidoProdutoMasculinoFeminino = $("#PedidoProdutoMasculinoFeminino");
    var pedidoProdutoTecido = $("#PedidoProdutoTecido");
    var pedidoProdutoTecidoCor = $("#PedidoProdutoTecidoCor");
    var pedidoProdutoTecidoCodigo = $("#PedidoProdutoTecidoCodigo");
    var precoProduto = $("#precoProduto");
    var obsProduto = $("#obsProduto");

    var pedidoProdutoAtual = $("#ProdutoPedido").text();
    var valido = true;
    var inputs = $(".inputRequiredProduto");

    inputs.each(function (index) {
        if (!Required($(inputs[index]))) {
            valido = false;
        }
    });

    if (!valido) {
        return false;
    }

    var produtoEdit = $("#produtoEdit").val();
    if (produtoEdit != 0) {
        RemoveProduto(produtoEdit);
        $("#produtoEdit").val(0);
    }

    var pedidoAtual = $("#pedidoAtual").text();

    $.ajax({
        'url': host + '/ParteAcrescimo/AdicionarProdutoDoPedido',
        'type': 'POST',
        'data': {
            'Quantidade': qtdeProduto.val(),
            'Peca': pedidoProdutoPeca.val(),
            'Modelo': pedidoProdutoModelo.val(),
            'MasculinoFeminino': pedidoProdutoMasculinoFeminino.val(),
            'Tecido': pedidoProdutoTecido.val(),
            'Cor': pedidoProdutoTecidoCor.val(),
            'Codigo': pedidoProdutoTecidoCodigo.val(),
            'Valor': precoProduto.val(),
            'Obs': obsProduto.val(),
            'Fornecedores': JSON.stringify(fornecedores),
            'pedidoProdutoAtual': pedidoProdutoAtual,
            'pedidoAtual': pedidoAtual,
            'Tamanhos': JSON.stringify(tamanhos)
        },
        'success': function (data) {
            if (data) {
                $("#collapseTwo .accordion-inner").html(data);
                $("#collapseOne .accordion-inner").html('');
                $("#collapseTwo").addClass("collapse").removeClass("in").css("height", "auto");
            }
        }
    });

    qtdeProduto.val('');
    pedidoProdutoPeca.val(0);
    pedidoProdutoModelo.val(0);
    pedidoProdutoMasculinoFeminino.val(0);
    pedidoProdutoTecido.val(0);
    pedidoProdutoTecidoCor.val(0);
    pedidoProdutoTecidoCodigo.val(0);
    precoProduto.val('');
    obsProduto.val('');
    checkBoxFornecedores.each(function () {
        $(this).prop("checked", false);
    });

    $('#tableTamanhos tbody .tamanhos').remove();
});

$("#btn_finalizarPedido").click(function () {
    var valido = true;
    var inputs = $(".inputRequired");

    inputs.each(function (index) {
        if (!Required($(inputs[index]))) {
            valido = false;
        }
    });

    if (valido) {
        var pedidoAtual = $("#pedidoAtual").text();
        $("#pedidoJson").val(pedidoAtual);
        $("form").submit();
    }

    return false;
});

$(".inputRequired").change(function () {
    Required($(this));
});

$(".inputRequiredProduto").change(function () {
    Required($(this));
});

function Required(domElement) {
    if (domElement.val() == 0 || domElement.val() == "" || domElement.val() === 'undefined') {
        domElement.css("border-color", "red");
        return false;
    } else {
        domElement.css("border-color", "");
        return true;
    }
}

function AcrescimoPorCategoriaId(idCategoria, rowNum) {
    $.ajax({
        'url': host + '/ParteAcrescimo/AcrescimoPorCategoriaIdEditar',
        'type': 'GET',
        'data': {
            'idCategoria': idCategoria,
            'rowNum': rowNum
        },
        'success': function (data) {
            if (data) {
                $("#Modal_Partes #ParteCategoria").html(data);
            }
        }
    });
}

$('body').on('click', '.editarParteAcrescimo', function () {
    var rowNum = $(this).attr("data-parte-acrescimo");

    $("#parteAcrescimoEdit").val(rowNum);

    CarregarTableItens(rowNum);
    CarregarSelectParteCategoria(rowNum);
    CarregarSelectTipoAviamento(rowNum);

    var quantidade = $(this).parent().parent().find('td:first').text().trim();
    var obs = $(this).parent().parent().find('td.Obs').text().trim();
    var labelModal = $(this).parent().parent().attr('data-categoria').split("-");;

    $("#quantidadeItemPeca").val(quantidade);
    $("#obsItemPeca").val(obs);
    $("#Modal_Partes #myModalLabel").text(labelModal[1]);
    $("#quantidadeAviamento").val('');
    $("#idCorAviamento").html("<option value='0'>Cor Aviamento</option>");
    $("#idCodigoAviamento").html("<option value='0'>Codigo Aviamento</option>");
    $("#checkboxParte").prop("checked", true);
    $("#Modal_Partes #tecidoItem").val('0');
    $("#Modal_Partes #corItem").val('0');
    $("#Modal_Partes #codigoItem").val('0');
    
    $("#Modal_Partes").modal('show');
});

function CarregarTableItens(rowNum) {
    var pedidoProduto = $("#ProdutoPedido").text();
    $.ajax({
        'url': host + '/ParteAcrescimo/CarregarTableItens',
        'type': 'POST',
        'data': {
            'rowNum' : rowNum,
            'pedidoProdutoAtual': pedidoProduto
        },
        'success': function (data) {
            if (data) {
                $("#tableItens").html(data);
                $("#tableItens").removeClass("hidden");
            }
        }
    });
}

$('body').on('click', '.removerParteAcrescimo', function () {
    var rowNum = $(this).attr("data-parte-acrescimo");
    RemoveParteAcrescimo(rowNum);
});

function CarregarSelectParteCategoria(rowNum) {
    var produtoPedido = $("#ProdutoPedido").text();
    $.ajax({
        'url': host + '/ParteAcrescimo/CarregarSelectParteCategoria',
        'type': 'POST',
        'data': {
            'rowNum': rowNum,
            'pedidoProdutoAtual': produtoPedido
        },
        'success': function (data) {
            if (data) {
                $("#ParteCategoria").html(data);
            }
        }
    });
}

function CarregarSelectTipoAviamento(rowNum) {
    var produtoPedido = $("#ProdutoPedido").text();
    $.ajax({
        'url': host + '/ParteAcrescimo/CarregarSelecTipoAviamento',
        'type': 'POST',
        'data': {
            'rowNum': rowNum,
            'pedidoProdutoAtual': produtoPedido
        },
        'success': function (data) {
            if (data) {
                $("#idTipoAviamento").html(data.Select);
                //RemoveParteAcrescimo(data.RowNum);
            }
        }
    });
}

function RemoveParteAcrescimo(rowNum) {
    var pedidoProdutoAtual = $("#ProdutoPedido").text();
    $.ajax({
        'url': host + '/ParteAcrescimo/RemoverParteAcrescimo',
        'type': 'POST',
        'data': {
            'rowNum': rowNum,
            'pedidoProdutoAtual': pedidoProdutoAtual
        },
        'success': function (data) {
            if (data) {
                $("#collapseOne .accordion-inner").html(data);
            }
        }
    });
}

$('body').on('click', '.editarProdutoDoPedido', function () {
    var linha = $(this);
    var rowNum = linha.attr("data-produto");

    var quantidade = linha.parent().parent().find('.quantidade').attr('data-quantidade');
    var peca = linha.parent().parent().find('.peca').attr('data-peca');
    var modelo = linha.parent().parent().find('.peca').attr('data-modelo');
    var modeloDescricao = linha.parent().parent().find('.peca').attr('data-modelo-descricao');
    var masculinoFeminino = linha.parent().parent().find('.mf').attr('data-mf');
    var tipoTecido = linha.parent().parent().find('.tecido').attr('data-tecido-id');
    var tecidoCorId = linha.parent().parent().find('.tecido').attr('data-cor-id');
    var tecidoCorDescricao = linha.parent().parent().find('.tecido').attr('data-cor-descricao');
    var tecidoCodigoId = linha.parent().parent().find('.tecido').attr('data-codigo-id');
    var tecidoCodigoDescricao = linha.parent().parent().find('.tecido').attr('data-codigo-descricao');
    var precoProduto = linha.parent().parent().find('.valor').attr('data-valor');
    var obsProduto = linha.parent().parent().find('.obs').attr('data-obs');

    $("#qtdeProduto").val(quantidade);
    $("#PedidoProdutoPeca").val(peca);
    $("#PedidoProdutoModelo").html("<option value='" + modelo + "'>" + modeloDescricao + "</option>");
    $("#PedidoProdutoMasculinoFeminino").val(masculinoFeminino);
    $("#PedidoProdutoTecido").val(tipoTecido);
    $("#PedidoProdutoTecidoCor").html("<option value='" + tecidoCorId + "'>" + tecidoCorDescricao + "</option>");
    $("#PedidoProdutoTecidoCodigo").html("<option value='" + tecidoCodigoId + "'>" + tecidoCodigoDescricao + "</option>");
    $("#precoProduto").val(precoProduto);
    $("#obsProduto").val(obsProduto);

    EditarParteAcrescimoProduto(rowNum);
    EditarFornecedoresProdutoPedido(rowNum);

    $("#produtoEdit").val(rowNum);
    //linha.parent().parent().remove();
});

$('body').on('click', '.verProdutoDoPedido', function() {
    var linha = $(this);
    var rowNum = linha.attr("data-produto");

    var pedidoAtual = $("#pedidoAtual").text();

    $.ajax({
        'url': host + '/ParteAcrescimo/MostrarItens',
        'type': 'POST',
        'data': {
            'rowNum': rowNum,
            'pedidoAtual': pedidoAtual
        },
        'success': function (data) {
            if (data) {
                $("#Modal_Itens .modal-body").html(data);
            }
        }
    });

    $("#Modal_Itens").modal('show');

});

function EditarParteAcrescimoProduto(rowNum) {
    var pedidoAtual = $("#pedidoAtual").text();

    $.ajax({
        'url': host + '/ParteAcrescimo/EditarParteAcrescimoProduto',
        'type': 'POST',
        'data': {
            'rowNum': rowNum,
            'pedidoAtual': pedidoAtual
        },
        'success': function (data) {
            if (data) {
                $("#collapseOne .accordion-inner").html(data);
            }
        }
    });
}

function EditarFornecedoresProdutoPedido(rowNum) {

    var pedidoAtual = $("#pedidoAtual").text();

    $.ajax({
        'url': host + '/ParteAcrescimo/EditarFornecedorProdutoDoPedido',
        'type': 'POST',
        'data': {
            'rowNum': rowNum,
            'pedidoAtual': pedidoAtual
        },
        'success': function (data) {
            if (data) {
                $("#divFornecedores").html(data.Select);
                EditarTamanhoProdutoDoPedido(data.RowNum);
            }
        }
    });
}

function EditarTamanhoProdutoDoPedido(rowNum) {

    var pedidoAtual = $("#pedidoAtual").text();

    $.ajax({
        'url': host + '/ParteAcrescimo/EditarTamanhoProdutoDoPedido',
        'type': 'POST',
        'data': {
            'rowNum': rowNum,
            'pedidoAtual': pedidoAtual
        },
        'success': function (data) {
            if (data) {
                $('#tableTamanhos tbody .tamanhos').remove();
                $('#tableTamanhos tbody').append(data.Select);
                //RemoveProduto(rowNum);
            }
        }
    });
}

$('body').on('click', '.removerProdutoDoPedido', function () {
    var linha = $(this);
    var rowNum = linha.attr("data-produto");
    linha.parent().parent().remove();

    RemoveProduto(rowNum);
});

function RemoveProduto(rowNum) {
    var pedidoAtual = $("#pedidoAtual");
    var jsonObj = JSON.parse(pedidoAtual.text());
    var indice = 0;

    $(jsonObj.PedidoProduto).each(function (i) {
        if (rowNum == jsonObj.PedidoProduto[i].RowNum) {
            indice = i;
        }
    });

    jsonObj.PedidoProduto.splice(indice, 1);
    var jsonStr = JSON.stringify(jsonObj);
    pedidoAtual.text(jsonStr);
}

function RemoveTamanho(tamanho) {
    $(tamanho).parent().parent().remove();
}

function onlyNumber(fields) {
    $(fields).unbind('keyup').bind('keyup', function (e) { //vou buscar o evento keyup - quando o usuário solta a tecla
        var thisVal = $(this).val(); // atribuo o valor do campo a variável local
        var tempVal = "";
        for (var i = 0; i < thisVal.length; i++) {
            if (RegExp(/^[0-9]$/).test(thisVal.charAt(i))) { // aqui estou usando uma expressão regular para limitar a entrada de apenas numeros ou seja digitos entre 0 e 9
                tempVal += thisVal.charAt(i); //caso atenda a condição de ser um digito numérico, atribuo a uma variável temporária
                if (e.keyCode == 8) {
                    tempVal = thisVal.substr(0, i); //o keyCode == 8 é para eu poder usar o backspace para apagar algum numero indesejado.
                }
            }
        }
        $(this).val(tempVal); // ao terminar, atribuo o valor validado ao valor do campo passado para testar
    });
}
onlyNumber($(".sonumero")); // a chamada a função fica assim, é só passar o campo que deseja ser validdo