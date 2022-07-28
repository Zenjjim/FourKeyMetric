import { COLORS } from "const";
import * as d3 from "d3";
import {
  CartesianGrid,
  Line,
  LineChart,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";
import { IChangeFailureRate } from "types";
import { getDateOfWeek } from "utils";

type ChangeFailureRateProps = {
  data: IChangeFailureRate;
  months: number;
};

export function ChangeFailureRate({ data, months }: ChangeFailureRateProps) {
  const getmontlyTransformedDataMedian = (data: IChangeFailureRate) =>
    data.changeFailureRateByMonth.map((d) => ({
      date: new Date(d.key.yearNumber, d.key.monthNumber),
      median: d.changeFailureRate * 100,
    }));
  const getweeklyTransformedDataMedian = (data: IChangeFailureRate) =>
    data.changeFailureRateByWeek.map((d) => ({
      date: getDateOfWeek(d.key.weekNumber, d.key.yearNumber),
      median: d.changeFailureRate * 100,
    }));

  const getDailyTransformedDataMedian = (data: IChangeFailureRate) =>
    data.changeFailureRateByDay.map((d) => ({
      date: new Date(d.key.yearNumber, d.key.monthNumber, d.key.dayNumber),
      median: d.changeFailureRate * 100,
    }));

  const transformedDataMedian =
    months < 2
      ? getDailyTransformedDataMedian(data)
      : months < 6
      ? getweeklyTransformedDataMedian(data)
      : getmontlyTransformedDataMedian(data);

  const CustomTooltip = ({
    active,
    payload,
    label,
  }: {
    active: boolean | undefined;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    payload: Array<any>;
    label: Date | undefined;
  }) => {
    if (active && payload && payload.length) {
      return (
        <div
          style={{
            backgroundColor: COLORS.PAPER,
            padding: "5px",
            borderRadius: "10px",
          }}
        >
          <p className="label">{`Failure Rate : ${
            Math.round(payload[0].value) + "%"
          }`}</p>
          <p className="date">{`Date : ${(label as Date).toDateString()}`}</p>
        </div>
      );
    }
    return null;
  };

  return (
    <ResponsiveContainer height="100%" width="100%">
      <LineChart
        margin={{
          top: 50,
          right: 50,
          bottom: 50,
          left: 50,
        }}
      >
        <text
          dominantBaseline="central"
          fill={COLORS.WHITE}
          textAnchor="middle"
          x={200}
          y={20}
        >
          <tspan fontSize="20" fontWeight="bolder">
            Change Failure Rate
          </tspan>
        </text>

        <XAxis
          dataKey="date"
          label={{
            value: "Date",
            dx: 0,
            dy: 20,
            fill: COLORS.WHITE,
            opacity: 0.75,
          }}
          opacity={0.75}
          stroke={COLORS.WHITE}
          tickFormatter={d3.timeFormat("%d %B")}
        />
        <YAxis
          dataKey="median"
          label={{
            value: "Percentage %",
            angle: -90,
            dx: -30,
            dy: 0,
            fill: COLORS.WHITE,
            opacity: 0.75,
          }}
          opacity={0.75}
          stroke={COLORS.WHITE}
        />
        <Line
          data={transformedDataMedian}
          dataKey="median"
          dot={true}
          isAnimationActive={false}
          stroke={COLORS.BLUE}
          strokeWidth={3}
          type="monotone"
        />
        <CartesianGrid opacity={0.25} stroke={COLORS.WHITE} vertical={false} />
        <Tooltip
          content={
            <CustomTooltip active={undefined} label={undefined} payload={[]} />
          }
          cursor={{ fill: "rgba(255, 255, 255, 0.1)" }}
        />
      </LineChart>
    </ResponsiveContainer>
  );
}
