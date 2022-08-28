-- public.attachments definition

-- Drop table

-- DROP TABLE public.attachments;

CREATE TABLE public.attachments (
                                    id bigserial NOT NULL,
                                    "name" varchar(256) NOT NULL,
                                    content_type text NOT NULL DEFAULT 'application/octet-stream'::text,
                                    "content" bytea NOT NULL,
                                    "key" text NOT NULL,
                                    CONSTRAINT attachments_pk PRIMARY KEY (id)
);


-- public.users definition

-- Drop table

-- DROP TABLE public.users;

CREATE TABLE public.users (
                              id bigserial NOT NULL,
                              "name" varchar(50) NOT NULL,
                              email_address text NOT NULL,
                              account_creation_time timestamp NOT NULL DEFAULT now(),
                              is_banned bool NOT NULL DEFAULT false,
                              ban_reason text NOT NULL DEFAULT ''::text,
                              settings jsonb NOT NULL DEFAULT '{}'::jsonb,
                              avatar bytea NULL,
                              "password" text NOT NULL,
                              CONSTRAINT users_pk PRIMARY KEY (id),
                              CONSTRAINT users_un UNIQUE (name)
);


-- public.login_history definition

-- Drop table

-- DROP TABLE public.login_history;

CREATE TABLE public.login_history (
                                      id bigserial NOT NULL,
                                      ip inet NULL,
                                      "time" timestamp NOT NULL DEFAULT now(),
                                      users_id bigserial NOT NULL,
                                      CONSTRAINT login_history_pk PRIMARY KEY (id),
                                      CONSTRAINT login_history_fk FOREIGN KEY (users_id) REFERENCES public.users(id) ON DELETE CASCADE
);


-- public.messages definition

-- Drop table

-- DROP TABLE public.messages;

CREATE TABLE public.messages (
                                 id int8 NOT NULL DEFAULT nextval('mesages_id_seq'::regclass),
                                 "content" text NOT NULL DEFAULT ''::text,
                                 "time" timestamp NOT NULL DEFAULT now(),
                                 source_id int8 NOT NULL DEFAULT nextval('mesages_source_id_seq'::regclass),
                                 target_id int8 NOT NULL DEFAULT nextval('mesages_target_id_seq'::regclass),
                                 CONSTRAINT messages_pk PRIMARY KEY (id),
                                 CONSTRAINT messages_fk FOREIGN KEY (source_id) REFERENCES public.users(id) ON DELETE CASCADE,
                                 CONSTRAINT messages_fk_1 FOREIGN KEY (target_id) REFERENCES public.users(id) ON DELETE CASCADE
);


-- public.messages_has_attachments definition

-- Drop table

-- DROP TABLE public.messages_has_attachments;

CREATE TABLE public.messages_has_attachments (
                                                 messages_id bigserial NOT NULL,
                                                 attachments_id bigserial NOT NULL,
                                                 CONSTRAINT messages_has_attachments_pk PRIMARY KEY (messages_id, attachments_id),
                                                 CONSTRAINT messages_has_attachments_fk FOREIGN KEY (messages_id) REFERENCES public.messages(id) ON DELETE CASCADE,
                                                 CONSTRAINT messages_has_attachments_fk_1 FOREIGN KEY (attachments_id) REFERENCES public.attachments(id) ON DELETE CASCADE
);