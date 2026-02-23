export interface IpaginationModel
{
  pageNumber: number;
  pageSize: number;
  totalRecords?: number;
  totalPages?: number;
}

export interface IPagedResult<T>
{
  pagingInfo: IpaginationModel;
  results:T[]
}
