
/*
{
	"Barebone": {},
	"Chinook": {
		"Administrator": [0, 1, 2, 4],
		"User": [0, 1]
	},
	"Security": {
		"User": [0, 1, 2],
		"Administrator": [0, 1, 2, 4],
		"Anonymous": [0]
	},
	"Extension1": {},
	"Extension2": {},
	"SeedDatabase": {}
}
*/

/*
{"id": "2",
                        "extension_name": "Extension de la tarte à poils",
                        "extension_admin": " ",
                        "extension_write": " ",
                        "extension_read": " ",
                        "roles_list": {
                           "Security": {
                                "User": [0, 1, 2],
                                "Administrator": [0, 1, 2, 4],
                                "Anonymous": [0]
                            }
                        }
                    }
                    */


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
