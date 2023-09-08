export interface IItemRequestDto {
  name: string;
  description: string;
  photoUrl: string;
  price: number;
  bid: number;
  isSellable: boolean;
  sellerId: number;
  creationDate: string;
  updateDate: string;
}
