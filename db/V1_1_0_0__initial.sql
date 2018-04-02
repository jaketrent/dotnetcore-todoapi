begin;

create sequence todo_id_seq
start with 1
increment by 1
no minvalue
no maxvalue
cache 1;

create table todo (
id int primary key default nextval('todo_id_seq'),
description text not null,
created date default now()
);

end;
