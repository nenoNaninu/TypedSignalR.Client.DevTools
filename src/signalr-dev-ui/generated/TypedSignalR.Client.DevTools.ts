/* THIS (.ts) FILE IS GENERATED BY Tapper */
/* eslint-disable */
/* tslint:disable */

/** Transpiled from TypedSignalR.Client.DevTools.SignalRService */
export type SignalRService = {
    /** Transpiled from string */
    name: string;
    /** Transpiled from string */
    path: string;
    /** Transpiled from bool */
    isAuthRequired: boolean;
    /** Transpiled from TypedSignalR.Client.DevTools.TypeMetadata */
    hubType: TypeMetadata;
    /** Transpiled from TypedSignalR.Client.DevTools.TypeMetadata */
    receiverType: TypeMetadata;
}

/** Transpiled from TypedSignalR.Client.DevTools.TypeMetadata */
export type TypeMetadata = {
    /** Transpiled from string */
    interfaceName: string;
    /** Transpiled from string */
    interfaceFullName: string;
    /** Transpiled from string */
    collisionFreeName: string;
    /** Transpiled from System.Collections.Generic.IReadOnlyList<TypedSignalR.Client.DevTools.MethodMetadata> */
    methods: MethodMetadata[];
}

/** Transpiled from TypedSignalR.Client.DevTools.MethodMetadata */
export type MethodMetadata = {
    /** Transpiled from string */
    methodName: string;
    /** Transpiled from string */
    returnType: string;
    /** Transpiled from bool */
    isGenericReturnType: boolean;
    /** Transpiled from string? */
    genericReturnTypeArgument?: string;
    /** Transpiled from System.Collections.Generic.IReadOnlyList<TypedSignalR.Client.DevTools.ParameterMetadata> */
    parameters: ParameterMetadata[];
}

/** Transpiled from TypedSignalR.Client.DevTools.ParameterMetadata */
export type ParameterMetadata = {
    /** Transpiled from string */
    name: string;
    /** Transpiled from string */
    typeName: string;
}

