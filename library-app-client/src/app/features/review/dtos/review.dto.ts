export interface ReviewDto {
  id: number;
  userId: number;
  userFullName: string;
  content: string;
  rating: number;
  createdAt: string;
}