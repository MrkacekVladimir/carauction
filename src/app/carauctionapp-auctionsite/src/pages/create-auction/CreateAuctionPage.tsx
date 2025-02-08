import { Link, useNavigate } from "react-router";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { useCreateAuctionMutation } from "./useCreateAuctionMutation";
import { Input, InputGroup, InputError } from "@/components/forms";

const createAuctionFormSchema = z
  .object({
    title: z.string(),
    startsOn: z.date().refine((date) => date >= new Date(), {
      message: "Start date must not be in the past",
    }),
    endsOn: z.date().refine((date) => date >= new Date(), {
      message: "End date must not be in the past",
    }),
  })
  .refine((data) => data.startsOn < data.endsOn, {
    message: "Start date must be before end date",
    path: ["endsOn"],
  });

type CreateAuctionForm = z.infer<typeof createAuctionFormSchema>;

export function CreateAuctionPage() {
  const {
    formState: { errors },
    register,
    handleSubmit,
  } = useForm<CreateAuctionForm>({
    resolver: zodResolver(createAuctionFormSchema),
  });
  const createMutation = useCreateAuctionMutation();
  const navigate = useNavigate();

  const handleFormSubmit = async (data: CreateAuctionForm) => {
    const response = await createMutation.mutateAsync(data);
    if (response.status >= 200 && response.status < 300) {
      navigate("/auctions");
    }
  };
  return (
    <div>
      <h1>Create auction</h1>
      <form onSubmit={handleSubmit(handleFormSubmit)}>
        <InputGroup>
          <label>Title</label>
          <Input {...register("title")} />
          {errors.title && <InputError>{errors.title.message}</InputError>}
        </InputGroup>
        <InputGroup>
          <label>Starts On</label>
          <Input
            {...register("startsOn", { valueAsDate: true })}
            type="datetime-local"
          />
          {errors.startsOn && (
            <InputError>{errors.startsOn.message}</InputError>
          )}
        </InputGroup>
        <InputGroup>
          <label>Ends On</label>
          <Input
            {...register("endsOn", { valueAsDate: true })}
            type="datetime-local"
          />
          {errors.endsOn && <InputError>{errors.endsOn.message}</InputError>}
        </InputGroup>
        <div>
          <button disabled={createMutation.isPending}>Create</button>
          <Link to={"/auctions"}>
            <button type="button">Back</button>
          </Link>
        </div>
      </form>
    </div>
  );
}
