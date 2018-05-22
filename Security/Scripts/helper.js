var Formatter = (function () {
    function Formatter(index, extensionData) {
        this.extension_name = " ";
        this.extension_admin = " ";
        this.extension_write = " ";
        this.extension_read = " ";
        this.roles_list = {};
        this.id = index;
        var extensionName = Object.keys(extensionData)[0];
        this.roles_list[extensionName] = {};
        Object.assign(this.roles_list[extensionName], extensionData[extensionName]);
    }
    return Formatter;
}());
var extensionData = { "Security": {
        "User": [0, 1, 2],
        "Administrator": [0, 1, 2, 4],
        "Anonymous": [0]
    }
};
var test = new Formatter(1, extensionData);
console.log(JSON.stringify(test));
//# sourceMappingURL=helper.js.map