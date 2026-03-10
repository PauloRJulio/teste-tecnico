export interface BaseResponse<T = any> {
    success: boolean
    message?: string
    status?: string
    item?: T
}