Object.defineProperty(Array.prototype, 'flatBy', {
    value: function (property) {
        var result = [];
        this.forEach(function (item) {
            result.push(item);
            if (item[property] && item[property].length > 0 && Array.isArray(item[property])) {
                result = result.concat(item[property].flatBy(property));
            }
        });
        return result;
    }
});
Object.defineProperty(Array.prototype, 'distinctBy', {
    value: function (property) {
        var flags = [], output = [], l = this.length, i;
        for (i = 0; i < l; i++) {
            if (flags[this[i][property]]) continue;
            flags[this[i][property]] = true;
            output.push(this[i]);
        }
        return output;
    }
});