const validateEmail = (email) => {

    let result = false;

    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email)) {

        result = true;
    }

    return result;
}

const sleep = (milliseconds) => new Promise(resolve => setTimeout(resolve, milliseconds));

const toPascalCase = (string) =>
    `${string}`.replace(new RegExp(/[-_]+/, 'g'), ' ').
        replace(new RegExp(/[^\w\s]/, 'g'), '').
        replace(
            new RegExp(/\s+(.)(\w+)/, 'g'),
            ($1, $2, $3) => `${$2.toUpperCase() + $3.toLowerCase()}`
        ).
        replace(new RegExp(/\s/, 'g'), '').
        replace(new RegExp(/\w/), s => s.toUpperCase());


export const otherTool = {
    validateEmail,
    sleep,
    toPascalCase
}

export default otherTool;