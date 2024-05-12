/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { Person } from '../models/Person';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class PersonService {
    /**
     * @param id
     * @returns Person Success
     * @throws ApiError
     */
    public static getApiV1Person(
        id: string,
    ): CancelablePromise<Person> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/v1/person/{Id}',
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
    public static deleteApiV1Person(
        id: string,
    ): CancelablePromise<void> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/v1/person/{Id}',
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
     * @param minAge
     * @param maxAge
     * @param sort
     * @param offset
     * @param limit
     * @returns Person Success
     * @throws ApiError
     */
    public static getApiV1Person1(
        name?: string,
        minAge?: number,
        maxAge?: number,
        sort?: string,
        offset?: number,
        limit?: number,
    ): CancelablePromise<Array<Array<Person>>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/v1/person',
            query: {
                'name': name,
                'minAge': minAge,
                'maxAge': maxAge,
                'sort': sort,
                'offset': offset,
                'limit': limit,
            },
        });
    }
    /**
     * @param requestBody
     * @returns Person Created
     * @throws ApiError
     */
    public static postApiV1Person(
        requestBody?: Person,
    ): CancelablePromise<Person> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/v1/person',
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad Request`,
            },
        });
    }
    /**
     * @param requestBody
     * @returns Person Success
     * @throws ApiError
     */
    public static putApiV1Person(
        requestBody?: Person,
    ): CancelablePromise<Person> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/v1/person',
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad Request`,
            },
        });
    }
}
