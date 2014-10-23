function formatDate(dateString) {
    var formattedDate = new Date(dateString);
    var d = formattedDate.getDate();
    var m = formattedDate.getMonth();
    m += 1;  // JavaScript months are 0-11
    var y = formattedDate.getFullYear();

    // prepend single day/month with zero
    d = ('0' + d).slice(-2);
    m = ('0' + m).slice(-2);

    return d + "/" + m + "/" + y;
}

function covertToISODateString(dateString) {
    var ISODate = parseDate(dateString);
    var d = ISODate.getDate();
    var m = ISODate.getMonth();
    m += 1;  // JavaScript months are 0-11
    var y = ISODate.getFullYear();

    // The .NET framework expecs yyyy-MM-dd
    return y + "-" + m + "-" + d;
}

// parse a date from dd/mm/yyyy format
function parseDate(input) {
    var parts = input.split('/');
    // new Date(year, month [, day [, hours[, minutes[, seconds[, ms]]]]])
    return new Date(parts[2], parts[1] - 1, parts[0]); // Note: months are 0-based
}

function getDateFromJSONString(JSONString) {
    var dateString = JSONString.match(/[0-9]+/g)[0];
    var formattedDate = new Date(parseInt(dateString, 10));
    var d = formattedDate.getDate();
    var m = formattedDate.getMonth();
    m += 1;  // JavaScript months are 0-11
    var y = formattedDate.getFullYear();

    // prepend single day/month with zero
    d = ('0' + d).slice(-2);
    m = ('0' + m).slice(-2);

    return d + "/" + m + "/" + y;
}