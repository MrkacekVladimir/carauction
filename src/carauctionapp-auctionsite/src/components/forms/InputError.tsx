import classes from "./InputError.module.css";

interface Props {
  children: React.ReactNode;
}
export function InputError({ children }: Props) {
  return <div className={classes.error}>{children}</div>;
}
