export type InputTextParameterTransformer = {
    (text: string): any;
}

const toBoolean: InputTextParameterTransformer = (text: string) => {
    const lower = text.toLowerCase();
    return lower === "true";
}

const toNumber: InputTextParameterTransformer = (text: string) => Number(text);
const toString: InputTextParameterTransformer = (text: string) => text;
const toDate: InputTextParameterTransformer = (text: string) => new Date(text);
const toObject: InputTextParameterTransformer = (text: string) => {

    if (!text) {
        return null;
    }

    return JSON.parse(text);
}

const dic: { [key: string]: InputTextParameterTransformer } = {
    "bool": toBoolean,
    "char": toString,
    "byte": toNumber,
    "sbyte": toNumber,
    "decimal": toNumber,
    "double": toNumber,
    "float": toNumber,
    "int": toNumber,
    "uint": toNumber,
    "long": toNumber,
    "ulong": toNumber,
    "short": toNumber,
    "ushort": toNumber,
    "byte[]": toString, // base64 string
    "string": toString,
    "global::System.Uri": toString,
    "global::System.Guid": toString,
    "global::System.DateTime": toDate,
    "global::System.DateTimeOffset": toDate,
}

export const getInputTextParameterTransformer = (text: string): InputTextParameterTransformer => {
    if (text in dic) {
        return dic[text];
    }
    return toObject;
}
