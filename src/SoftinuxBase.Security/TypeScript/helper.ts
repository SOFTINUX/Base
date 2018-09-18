class Formatter {
    id: number;
    extension_name: String = " ";
    extension_admin: String = " ";
    extension_write: String = " ";
    extension_read: String = " ";
    roles_list: any = {};

constructor(index:number, extensionData: any) {
    this.id = index;
    // extension name
    let extensionName: any = Object.keys(extensionData)[0];
    this.roles_list[extensionName] = {};
    // copy role settings
    Object.assign(this.roles_list[extensionName], extensionData[extensionName]);

}

}

let extensionData: any = {"Security": {
    "User": [0, 1, 2],
    "Administrator": [0, 1, 2, 4],
    "Anonymous": [0]}
};

let test: Formatter = new Formatter(1, extensionData);

console.log(JSON.stringify(test));
