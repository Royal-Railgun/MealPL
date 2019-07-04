(function ($) {   
    var defaults = {
        valueSelector: "",
        valueSplitor: "~"
    };
    function arrEditor(options) {
        var settings = $.extend(true, {}, defaults, options);
        return this.each(function () {
            var $this = $(this);
            var valueList = $(settings.valueSelector, $this).val().split(settings.valueSplitor);
            var $arrList = $("<ul class='arreditor'></ul>");
            $.each(valueList, function (i, v) {
                if (!v) {
                    return true;
                }
                var $item = $('<li class="arreditor_item" data-value="' + v + '">'
                                 + '<span class="display_value">' + v + '</span>'
                                 + '<input type="text" value="' + v + '" class="input_value"/>'
                                 + '<div class="action">'
                                     + '<span class="edit">编辑</span>'
                                     + '<span class="remove">删除</span>'
                                     + '<span class="ok">确认</span>'
                                     + '<span class="cancel">取消</span>'
                                 + '</div>'
                             + '</li>');
                $item.hover(function () { $(this).addClass("hover"); }, function () { $(this).removeClass("hover"); });
                $arrList.append($item);
            });
            $arrList.append($('<li class="arreditor_newitem">添加</li>'))
            $this.append($arrList);
            $arrList.on("click", ".arreditor_newitem", function () {
                var $item = $('<li class="arreditor_item editing" data-value="">'
                                 + '<span class="display_value"></span>'
                                 + '<input type="text" value="" class="input_value"/>'
                                 + '<div class="action">'
                                     + '<span class="edit">编辑</span>'
                                     + '<span class="remove">删除</span>'
                                     + '<span class="ok">确认</span>'
                                     + '<span class="cancel">取消</span>'
                                 + '</div>'
                             + '</li>');
                $item.hover(function () { $(this).addClass("hover"); }, function () { $(this).removeClass("hover"); });
                $(this).before($item);
            });
            $arrList.on("click", ".arreditor_item .edit", function () {
                var $item = $(this).parents(".arreditor_item").eq(0);
                $item.addClass("editing");
            });
            $arrList.on("click", ".arreditor_item .remove", function () {
                if (confirm("确定删除该项目？")) {
                    var $item = $(this).parents(".arreditor_item").eq(0);
                    $item.remove();
                    refreshValue()
                }
            });

            $arrList.on("click", ".arreditor_item .ok", function () {
                var $item = $(this).parents(".arreditor_item").eq(0);
                var newValue = $(".input_value", $item).val();
                if (!newValue) {
                    alert("输入值不能为空");
                    return;
                }
                $item.removeClass("editing");
                $(".display_value", $item).text(newValue);
                $item.attr("data-value", newValue);
                refreshValue()
            });
            $arrList.on("click", ".arreditor_item .cancel", function () {
                var $item = $(this).parents(".arreditor_item").eq(0);
                var oldValue = $item.attr("data-value");
                if (!oldValue) {
                    $item.remove();
                }
                $item.removeClass("editing");
                $(".input_value", $item).val(oldValue);
            });

            function refreshValue() {
                var arrList = [];
                $(".arreditor_item", $arrList).each(function (i, v) {
                    arrList.push($(v).attr("data-value"));
                });

                $(settings.valueSelector).val(arrList.join(settings.valueSplitor));
            }
        });
    }
    $.fn.arrEditor = arrEditor;
    $.fn.arrEditor.defaults = defaults;
})(jQuery);