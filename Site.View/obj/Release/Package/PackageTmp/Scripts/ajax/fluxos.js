$(document).ready(function () {
    $(document).on('click', '#MarcarTodosProduto', function () {
        if ($(this).is(':checked')) {
            var linhas = $(".pedidoProduto_check").parent().parent();
            linhas.each(function () {
                if (!$(this).hasClass("hidden")) {
                    $(this).find(".pedidoProduto_check").prop('checked', true);
                }
            });
        }
        else {
            $(".pedidoProduto_check").prop('checked', false);
        }
    });

    $(document).on('click', '#QtdeTotalProduto', function () {

        var linhas = $(".trPedidoProduto");

        if ($(this).is(':checked')) {
            linhas.each(function () {
                if (!$(this).hasClass("hidden")) {
                    $(this).find(".tdPedidoProduto").val($(this).find(".tdPedidoProduto").attr('data-quantidade'));
                }
            });
        }
        else {
            linhas.each(function () {
                if (!$(this).hasClass("hidden")) {
                    $(this).find(".tdPedidoProduto").val(0);
                }
            });
        }
    });

    $(document).on('click', '#check_enviar', function () {
        var acaoCheck = $(this);
        if (acaoCheck.is(':checked')) {
            $("#dadosEnvio").removeClass("hide");
        } else {
            var retornar = $("#check_retornar").is(':checked');
            var outros = $("#check_outros").is(':checked');
            if (!retornar)
                if (!outros)
                    $("#dadosEnvio").addClass("hide");
        }
    });

    $(document).on('click', '#check_retornar', function () {
        var acaoCheck = $(this);
        if (acaoCheck.is(':checked')) {
            $("#dadosEnvio").removeClass("hide");
        } else {
            var enviar = $("#check_enviar").is(':checked');
            var outros = $("#check_outros").is(':checked');
            if (!enviar)
                if (!outros)
                    $("#dadosEnvio").addClass("hide");
        }
    });

    $(document).on('click', '#check_outros', function () {
        var acaoCheck = $(this);
        if (acaoCheck.is(':checked')) {
            $("#dadosEnvio").removeClass("hide");
        } else {
            var enviar = $("#check_enviar").is(':checked');
            var retornar = $("#check_retornar").is(':checked');
            if (!enviar)
                if (!retornar)
                    $("#dadosEnvio").addClass("hide");
        }
    });

    $(document).on('click', '#BtnLancarFluxo', function () {
        var pedidosCheck = $(".pedidos_check:checked");
        var etapasCheck = $(".etapas_check:checked");
        var acaoCheck = $(".acao_check:checked");

        var checkEnviar = $("#check_enviar:checked");
        var checkRetornar = $("#check_retornar:checked");
        var checkOutros = $("#check_outros:checked");

        if (pedidosCheck.length == 0) {
            jAlert('SELECIONE O PEDIDO!', 'Atenção');
            return false;
        }

        if (etapasCheck.length == 0) {
            jAlert('SELECIONE A ETAPA!', 'Atenção');
            return false;
        }

        if (acaoCheck.length == 0) {
            jAlert('SELECIONE A AÇÃO!', 'Atenção');
            return false;
        }

        if (checkEnviar.length > 0 || checkRetornar.length > 0 || checkOutros.length > 0) {
            if ($(".pedidoProduto_check:checked").length == 0) {
                jAlert('SELECIONE O PRODUTO!', 'Atenção');
                return false;
            } else {
                $(".pedidoProduto_check:checked").each(function (index) {
                    $(this).attr('name', 'PedidoProdutos['+index+'].Produto');
                    $("#PedidoProdutoQuantidade_" + $(this).val()).attr('name', 'PedidoProdutos[' + index + '].Quantidade');
                    $("#PedidoProdutoPedido_" + $(this).val()).attr('name', 'PedidoProdutos[' + index + '].Pedido');
                });
            }
        }

        $(".lancamentosAtualizar_check:checked").each(function (index) {
            $(this).attr('name', 'lancamentosAtualizar[' + index + ']');
        });

        return true;
    });

    $(document).on('change', '.pedidos_check', function () {
        var pedido = $(this);
        var classe = "pedido_" + pedido.val();

        if (pedido.is(':checked')) {
            $("." + classe).removeClass('hidden');
        } else {
            $("." + classe).addClass('hidden');
            $("." + classe + ' .pedidoProduto_check').removeAttr('checked');
            $(".pedidoFluxo_" + pedido.val()).addClass('hidden');
        }
    });

    $(document).on('click', '#link_pedidoEtapaAcao', function () {
        $(".fluxosLancados").addClass('hidden');
    });

    $(document).on('click', '#link_produtos', function () {
        $(".fluxosLancados").addClass('hidden');
    });

    $(document).on('click', '#link_lancamentos', function () {
        var pedidosCheck = $(".pedidos_check:checked");
        var etapasCheck = $(".etapas_check:checked");
        var acaoCheck = $(".acao_check:checked");
        var pedidoProdutoCheck = $(".pedidoProduto_check:checked");

        $(pedidosCheck).each(function () {
            var pedido = $(this).val();
            $(etapasCheck).each(function () {
                var etapa = $(this).val();
                $(acaoCheck).each(function () {
                    var acao = $(this).val();
                    if (acao == 2) {
                        $(".pedido_" + pedido + "_andamento_" + etapa + "_acao_" + acao + "_pedidoProduto_0").removeClass('hidden');
                    } else {
                        $(pedidoProdutoCheck).each(function () {
                            var pedidoProduto = $(this).val();
                            $(".pedido_" + pedido + "_andamento_" + etapa + "_acao_" + acao + "_pedidoProduto_" + pedidoProduto).removeClass('hidden');
                        });
                    }
                });
            });
        });
    });

});
