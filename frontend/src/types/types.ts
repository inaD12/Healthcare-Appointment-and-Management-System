import { z, ZodType } from "zod"

export interface APIResponse<T>{
  message:string;
  data: T;
}

export function PaginatedQueryResponseSchema<T extends ZodType<any, any, any>>(itemSchema: T) {
  return z.object({
    Items: z.array(itemSchema),
    Page: z.number().int(),
    PageSize: z.number().int(),
    TotalCount: z.number().int(),
    HasNextPage: z.boolean(),
    HasPreviousPage: z.boolean(),
  })
}