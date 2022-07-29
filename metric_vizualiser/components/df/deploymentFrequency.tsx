import { COLORS } from "const";
import * as d3 from "d3";
import {
  Bar,
  BarChart,
  CartesianGrid,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";
import { Deployment, IDeploymentFrequency } from "types";
import { getDateOfWeek } from "utils";
type DeploymentFrequencyProps = {
  data: IDeploymentFrequency;
  months: number;
};
export function DeploymentFrequency({
  data,
  months,
}: DeploymentFrequencyProps) {
  const daily = 2;
  const weekly = 6;
  const getDaily = (data: IDeploymentFrequency) => {
    return data?.deployments.map((deployment) => {
      deployment["date"] = new Date(
        deployment.yearNumber,
        deployment.monthNumber - 1,
        deployment.dayNumber
      );
      deployment["count"] = deployment.deploymentsInBucket.length;
      return deployment;
    });
  };

  const getWeekly = (data: IDeploymentFrequency) => {
    const temp: Deployment[] = [];
    data?.deployments.forEach((deployment) => {
      const found = temp.find(
        (t) =>
          t.weekNumber === deployment.weekNumber &&
          t.yearNumber === deployment.yearNumber
      );
      if (found) {
        found.deploymentsInBucket = found.deploymentsInBucket.concat(
          deployment.deploymentsInBucket
        );
      } else {
        temp.push({
          date: getDateOfWeek(
            deployment.weekNumber as number,
            deployment.yearNumber
          ),
          weekNumber: deployment.weekNumber,
          monthNumber: deployment.monthNumber,
          yearNumber: deployment.yearNumber,
          deploymentsInBucket: deployment.deploymentsInBucket,
        });
      }
    });
    return temp.map((deployment) => {
      deployment["count"] = deployment.deploymentsInBucket.length;
      return deployment;
    });
  };
  const getMonthly = (data: IDeploymentFrequency) => {
    const temp: Deployment[] = [];
    data?.deployments.forEach((deployment) => {
      const found = temp.find(
        (t) =>
          t.monthNumber === deployment.monthNumber &&
          t.yearNumber === deployment.yearNumber
      );
      if (found) {
        found.deploymentsInBucket = found.deploymentsInBucket.concat(
          deployment.deploymentsInBucket
        );
      } else {
        temp.push({
          date: new Date(deployment.yearNumber, deployment.monthNumber - 1, 1),
          monthNumber: deployment.monthNumber,
          yearNumber: deployment.yearNumber,
          deploymentsInBucket: deployment.deploymentsInBucket,
        });
      }
    });
    return temp.map((deployment) => {
      deployment["count"] = deployment.deploymentsInBucket.length;
      return deployment;
    });
  };
  const displayData: Deployment[] =
    months < daily
      ? getDaily(data)
      : months < weekly
      ? getWeekly(data)
      : getMonthly(data);

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
          <p className="label">{`Deployments : ${payload[0].value}`}</p>
          <p className="date">{`Date : ${(label as Date).toDateString()}`}</p>
        </div>
      );
    }
    return null;
  };

  return (
    <ResponsiveContainer height="100%" width="100%">
      <BarChart
        data={displayData}
        margin={{
          top: 50,
          right: 50,
          bottom: 30,
          left: 30,
        }}
      >
        <CartesianGrid vertical={false} />
        <text
          dominantBaseline="central"
          fill={COLORS.WHITE}
          textAnchor="start"
          x={90}
          y={20}
        >
          <tspan fontSize="20" fontWeight="bolder">
            Deployment Frequency
          </tspan>
        </text>
        <XAxis
          dataKey="date"
          label={{
            value: "Date",
            dx: 0,
            dy: 25,
            fill: COLORS.WHITE,
            opacity: 0.75,
          }}
          opacity={0.75}
          stroke={COLORS.WHITE}
          tickFormatter={d3.timeFormat("%d %B")}
        />
        <YAxis
          label={{
            value: "Deploys",
            angle: -90,
            dx: -30,
            dy: 0,
            fill: COLORS.WHITE,
            opacity: 0.75,
          }}
          opacity={0.75}
          stroke={COLORS.WHITE}
        />
        <Tooltip
          content={
            <CustomTooltip active={undefined} label={undefined} payload={[]} />
          }
          cursor={{ fill: "rgba(255, 255, 255, 0.1)" }}
        />
        <Bar dataKey="count" fill={COLORS.BLUE} />
      </BarChart>
    </ResponsiveContainer>
  );
}
