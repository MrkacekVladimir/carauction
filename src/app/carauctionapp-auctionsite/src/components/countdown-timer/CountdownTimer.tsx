import { useState, useEffect } from "react";
import { calculateTimeLeft } from "./utils";

interface Props {
  targetDate: Date;
}

export const CountdownTimer = ({ targetDate }: Props) => {
  const [timeLeft, setTimeLeft] = useState(calculateTimeLeft(targetDate));

  useEffect(() => {
    const timer = setTimeout(() => {
      setTimeLeft(calculateTimeLeft(targetDate));
    }, 1000);

    return () => clearTimeout(timer);
  });

  return (
    <div>
      {timeLeft && (
        <div>
          {timeLeft.days} days {timeLeft.hours}:{timeLeft.minutes}:
          {timeLeft.seconds}
        </div>
      )}
      {!timeLeft && <span>Time's up!</span>}
    </div>
  );
};
