import { z, ZodType } from "zod"

export interface APIResponse<T>{
  message:string;
  data: T;
}

export function PaginatedQueryResponseSchema<T extends ZodType<any, any, any>>(itemSchema: T) {
  return z.object({
    items: z.array(itemSchema),
    page: z.number().int(),
    pageSize: z.number().int(),
    totalCount: z.number().int(),
    hasNextPage: z.boolean(),
    hasPreviousPage: z.boolean(),
  })
}