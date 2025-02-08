import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { apiUrl } from "@/lib/api/config";
import { CreateBidRequest } from "@/lib/api/types";
import { Input, InputGroup, InputError } from "@/components/forms";

const newBidFormSchema = z.object({
  bidAmount: z.number().min(0, "Bid amount cannot be lower than 0"),
});

type NewBidForm = z.infer<typeof newBidFormSchema>;

interface Props {
  auctionId: string;
}

export function AuctionBidInput({ auctionId }: Props) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<NewBidForm>({
    resolver: zodResolver(newBidFormSchema),
  });

  const sendBid = async (data: NewBidForm) => {
    const model: CreateBidRequest = {
      amount: data.bidAmount,
    };

    await fetch(`${apiUrl}/auctions/${auctionId}/bids`, {
      headers: {
        "Content-Type": "application/json",
      },
      method: "POST",
      body: JSON.stringify(model),
    });
  };

  return (
    <form onSubmit={handleSubmit(sendBid)}>
      <InputGroup>
        <label>Amount</label>
        <Input
          {...register("bidAmount", { valueAsNumber: true })}
          type="number"
        />
        {errors.bidAmount && (
          <InputError>{errors.bidAmount.message}</InputError>
        )}
      </InputGroup>
      <button>Send bid</button>
    </form>
  );
}
