/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { Item } from '../models/Item';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class ItemService {
    /**
     * @param id
     * @returns Item Success
     * @throws ApiError
     */
    public static getApiV1Item(
        id: string,
    ): CancelablePromise<Item> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/v1/item/{Id}',
            path: {
                'id': id,
            },
            errors: {
                404: `Not Found`,
            },
        });
    }
    /**
     * @param id
     * @returns void
     * @throws ApiError
     */
    public static deleteApiV1Item(
        id: string,
    ): CancelablePromise<void> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/v1/item/{Id}',
            path: {
                'id': id,
            },
            errors: {
                400: `Bad Request`,
            },
        });
    }
    /**
     * @param name
     * @param description
     * @param personId
     * @param minPreco
     * @param maxPreco
     * @param sort
     * @param offset
     * @param limit
     * @returns Item Success
     * @throws ApiError
     */
    public static getApiV1Item1(
        name?: string,
        description?: string,
        personId?: string,
        minPreco?: number,
        maxPreco?: number,
        sort?: string,
        offset?: number,
        limit?: number,
    ): CancelablePromise<Array<Array<Item>>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/v1/item',
            query: {
                'name': name,
                'description': description,
                'personId': personId,
                'minPreco': minPreco,
                'maxPreco': maxPreco,
                'sort': sort,
                'offset': offset,
                'limit': limit,
            },
        });
    }
    /**
     * @param requestBody
     * @returns Item Created
     * @throws ApiError
     */
    public static postApiV1Item(
        requestBody?: Item,
    ): CancelablePromise<Item> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/v1/item',
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad Request`,
            },
        });
    }
    /**
     * @param requestBody
     * @returns Item Success
     * @throws ApiError
     */
    public static putApiV1Item(
        requestBody?: Item,
    ): CancelablePromise<Item> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/v1/item',
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad Request`,
            },
        });
    }
}
