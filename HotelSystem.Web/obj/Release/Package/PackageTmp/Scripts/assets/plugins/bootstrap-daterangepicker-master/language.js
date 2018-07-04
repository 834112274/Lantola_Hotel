locale = {
    "direction": "ltr",
    "format": "YYYY年MM月DD日",
    "separator": "~",
    "applyLabel": "确认",
    "cancelLabel": "取消",
    "fromLabel": "From",
    "toLabel": "To",
    "customRangeLabel": "Custom",
    "daysOfWeek": [
        "日",
        "一",
        "二",
        "三",
        "四",
        "五",
        "六" 
    ],
    "monthNames": [
        "一月",
         "二月",
         "三月",
         "四月",
         "五月",
         "六月",
         "七月",
         "八月",
         "九月",
         "十月",
         "十一月",
         "十二月"
    ],
    "firstDay": 1
};

var getWeek = function (number) {
    switch (number) {
        case "0": return "周日";
        case "1": return "周一";
        case "2": return "周二";
        case "3": return "周三";
        case "4": return "周四";
        case "5": return "周五";
        case "6": return "周六";
    };
}
