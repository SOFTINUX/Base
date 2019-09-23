class CustomLogging {
    constructor(title) {
        this.title = {
            body: title || '---',
            color: 'darkgrey',
            size: '12px'
        };

        this.body = {
            color: '#grey',
            size: '12px'
        };

        this.successColor = {
            color: '#155724',
            background: '#d4edda'
        };

        this.infoColor = {
            color: '#31708f',
            background: '#d9edf7'
        };

        this.warnColor = {
            color: '#8a6d3b',
            background: '#fcf8e3'
        };

        this.errorColor = {
            color: '#a94442',
            background: '#f2dede'
        };
    }

    setTitleStyle({ color, size }) {
        if (color !== undefined) this.title.color = color;
        if (size !== undefined) this.title.size = size;
    }

    setBodyStyle({ color, size }) {
        if (color !== undefined) this.body.color = color;
        if (size !== undefined) this.body.size = size;
    }

    log(body = '') {
        console.log(
            `%c${this.title.body} | %c${body}`,
            `color: ${this.title.color}; font-weight: bold; font-size: ${this.title.size}; text-shadow: 0 0 5px rgba(0,0,0,0,2);`,
            `color: ${this.body.color}; font-weight: normal; font-size: ${this.body.size}; text-shadow: 0 0 5px rgba(0,0,0,0,2);`
        );
    }

    success(body = '') {
        console.log(
            `%c[\u2714] ${this.title.body} | %c${body}`,
            `color: ${this.successColor.color}; background-color: ${this.successColor.background}; font-weight: bold; font-size: ${this.title.size}; text-shadow: 0 0 5px rgba(0,0,0,0,2);`,
            `color: ${this.successColor.color}; background-color: ${this.successColor.background}; font-weight: normal; font-size: ${this.body.size}; text-shadow: 0 0 5px rgba(0,0,0,0,2);`
        );
    }

    warn(body = '') {
        console.log(
            `%c[\u26A0] ${this.title.body} | %c${body}`,
            `color: ${this.warnColor.color}; background-color: ${this.warnColor.background}; font-weight: bold; font-size: ${this.title.size}; text-shadow: 0 0 5px rgba(0,0,0,0,2);`,
            `color: ${this.warnColor.color}; background-color: ${this.warnColor.background}; font-weight: normal; font-size: ${this.body.size}; text-shadow: 0 0 5px rgba(0,0,0,0,2);`
        );
    }

    info(body = '') {
        console.log(
            `%c[\u2139] ${this.title.body} | %c${body}`,
            `color: ${this.infoColor.color}; background-color: ${this.infoColor.background}; font-weight: bold; font-size: ${this.title.size}; text-shadow: 0 0 5px rgba(0,0,0,0,2);`,
            `color: ${this.infoColor.color}; background-color: ${this.infoColor.background}; font-weight: normal; font-size: ${this.body.size}; text-shadow: 0 0 5px rgba(0,0,0,0,2);`
        );
    }

    error(body = '') {
        console.log(
            `%c[\u274C] ${this.title.body} | %c${body}`,
            `color: ${this.errorColor.color}; background-color: ${this.errorColor.background}; font-weight: bold; font-size: ${this.title.size}; text-shadow: 0 0 5px rgba(0,0,0,0,2);`,
            `color: ${this.errorColor.color}; background-color: ${this.errorColor.background}; font-weight: normal; font-size: ${this.body.size}; text-shadow: 0 0 5px rgba(0,0,0,0,2);`
        );
    }
}

export default CustomLogging;
