(function () {
    var util = {
        GetHash: function (value) {
            var hash = 0, i, char;
            if (value.length == 0) return hash;
            for (i = 0, l = value.length; i < l; i++) {
                char = value.charCodeAt(i);
                hash = ((hash << 5) - hash) + char;
                hash |= 0; // Convert to 32bit integer
            }
            return hash;
        },
        PropertyExist: function (name, obj) {
            var _propname = name.toUpperCase();
            for (var prop in obj) {
                if (obj.hasOwnProperty(prop)) {
                    if (prop.toUpperCase() == _propname) {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    if (!dbe.util) { dbe.util = util; }
})();